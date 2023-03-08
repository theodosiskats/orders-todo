using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;

var scopes = new[] { "User.Read" };

var tenantId = "cf6cda30-1730-43c0-925d-8c5aebfdf535";
var clientId = "cefafc21-5154-41ed-9b55-5b7080a226fc";
var ordersTodoList = "AAMkAGIwODg4NWMyLTk5YTgtNGE5Zi1iMjdjLTdhMDNmNzAyMjllMwAuAAAAAADCZnWuYygKQp1EKwyGLDx4AQA6tPYtcRxXSKIPcU6ONsCnAAJjE4_5AAA=";
var interactiveCredential = new InteractiveBrowserCredential(new InteractiveBrowserCredentialOptions
{
    TenantId = tenantId,
    ClientId = clientId,
    AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
    RedirectUri = new Uri("http://localhost"),
});

var graphClient = new GraphServiceClient(interactiveCredential, scopes);

//TODO : get todo lists

//find if Orders list exists
    //if exists get id
    //else 
        //create orders list
        //get task lists
        //get orders list id

// // Create Orders Todo list
// var requestBody = new TodoTaskList { DisplayName = $"Orders" };
// await graphClient.Me.Todo.Lists.PostAsync(requestBody);


var products = new List<string> { "Product A", "Product B", "Product C", "Product D", "Product E" };
string randomProduct = products[new Random().Next(products.Count)];
int orderNumber = new Random().Next(10000, 99999);
bool inStock = new Random().Next(2) != 0;
DateTime currentDate = DateTime.Now;
DateTime shippingDate = inStock ? currentDate.AddDays(3) : currentDate.AddDays(7);

var requestBody = new TodoTask
{
	Title = $"Order: {orderNumber}",
	Body = new ItemBody { Content = $"Product: {randomProduct} \n shippingDate: {shippingDate.ToShortDateString()} \n isInStock: {inStock.ToString()}" },
	Categories = new List<string>
	{
		"Orders",
	},
};

requestBody.Body.Content = $"Product: {randomProduct} \n shippingDate: {shippingDate.ToShortDateString()} \n isInStock: {inStock.ToString()}";

await graphClient.Me.Todo.Lists[$"{ordersTodoList}"].Tasks.PostAsync(requestBody);

