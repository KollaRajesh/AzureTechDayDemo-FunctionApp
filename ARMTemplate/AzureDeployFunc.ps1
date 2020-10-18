
$resourceGroup=Read-Host "Enter Resource group"
$location=Read-Host "Enter location"

New-AzResourceGroup -Name $resourceGroup -Location $location #use this command when you need to create a new resource group for your deployment
New-AzResourceGroupDeployment -ResourceGroupName $resourceGroup -TemplateUri https://raw.githubusercontent.com/Azure/azure-quickstart-templates/master/101-function-app-create-dynamic/azuredeploy.json