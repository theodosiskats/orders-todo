using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;

var scopes = new[] { "User.Read" };

const string tenantId = "cf6cda30-1730-43c0-925d-8c5aebfdf535";
const string clientId = "cefafc21-5154-41ed-9b55-5b7080a226fc";
var interactiveCredential = new InteractiveBrowserCredential(new InteractiveBrowserCredentialOptions
{
    TenantId = tenantId,
    ClientId = clientId,
    AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
    RedirectUri = new Uri("http://localhost"),
});

var graphClient = new GraphServiceClient(interactiveCredential, scopes);

var todoListCollection = await graphClient.Me.Todo.Lists.GetAsync();

//find or create the Orders & Stock TodoLists
var ordersTodoList = await FindOrCreateOrdersList(todoListCollection);
var stockTodoList = await FindOrCreateStockList(todoListCollection);

var newOrderInfo = GenerateNewOrder();

await AddOrderToMyLists(ordersTodoList.Id, stockTodoList.Id, newOrderInfo);

async Task<TodoTaskList> FindOrCreateOrdersList(TodoTaskListCollectionResponse todoListCollection)
{
    if (todoListCollection.Value.Count > 0)
    {
        foreach (var todoList in todoListCollection.Value)
        {
            if (todoList.DisplayName == "Orders")
            {
                return todoList;
            }
        }
    }
    
    // If the "Orders" list does not exist, create it
    var requestBody = new TodoTaskList
    {
        DisplayName = "Orders"
    };
    var createdTodoList = await graphClient.Me.Todo.Lists.PostAsync(requestBody);
    return createdTodoList;
}

async Task<TodoTaskList> FindOrCreateStockList(TodoTaskListCollectionResponse todoListCollection)
{
    if (todoListCollection.Value.Count > 0)
    {
        foreach (var todoList in todoListCollection.Value)
        {
            if (todoList.DisplayName == "Stock")
            {
                return todoList;
            }
        }
    }
    // If the "Stock" list does not exist, create it
    var requestBody = new TodoTaskList
    {
        DisplayName = "Stock"
    };
    var createdTodoList = await graphClient.Me.Todo.Lists.PostAsync(requestBody);
    return createdTodoList;
}

Order GenerateNewOrder()
{
    var products = new List<string> { "Product A", "Product B", "Product C", "Product D", "Product E" };
    string randomProduct = products[new Random().Next(products.Count)];
    int orderNumber = new Random().Next(001, 100);
    int quantity = new Random().Next(1, 10);
    bool inStock = new Random().Next(2) != 0;;
    DateTime currentDate = DateTime.Now;
    DateTime shippingDate;
    
    if (inStock)
    {
        shippingDate = currentDate.AddDays(3);
    }
    else
    {
        shippingDate = currentDate.AddDays(7);
    }

    // Check if the shipping date is on a Saturday or Sunday
    if (shippingDate.DayOfWeek == DayOfWeek.Saturday)
    {
        shippingDate = shippingDate.AddDays(2);
    }
    else if (shippingDate.DayOfWeek == DayOfWeek.Sunday)
    {
        shippingDate = shippingDate.AddDays(1);
    }
    
    var newOrder = new TodoTask
    {
        Title = $"Order: {orderNumber}",
        Body = new ItemBody { Content = $"Product: {randomProduct} \n Shipping Date: {shippingDate.ToShortDateString()} \n is In Stock: {inStock.ToString()}" },
        Categories = new List<string>
        {
            "Orders",
        },
        DueDateTime = shippingDate.ToDateTimeTimeZone()
    };

    //If product out of stock, create task to restock
    TodoTask stockInfo = new TodoTask();
    if(inStock == false)
    {
        stockInfo = new TodoTask
        {
            Title = $"Restock {randomProduct} | Quantity: {quantity} | Order: {orderNumber}",
            Body = new ItemBody { Content = $"Product: {randomProduct} \n Stock needed: {quantity}" },
            Categories = new List<string>
            {
                "Stock",
            },
            DueDateTime = shippingDate.ToDateTimeTimeZone()
        };
    }

    return new Order { OrderInfo = newOrder, StockInfo = stockInfo };
}

async Task AddOrderToMyLists(string ordersListId,string stockListId, Order newOrderInfo)
{
    await graphClient.Me.Todo.Lists[$"{ordersListId}"].Tasks.PostAsync(newOrderInfo.OrderInfo);
    await graphClient.Me.Todo.Lists[$"{stockListId}"].Tasks.PostAsync(newOrderInfo.StockInfo);
}

public class Order
{
    public TodoTask OrderInfo { get; set; }
    public TodoTask StockInfo { get; set; }
}
