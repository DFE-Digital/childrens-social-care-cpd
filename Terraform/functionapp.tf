resource "azurerm_storage_account" "functionappsa" {
  name                     = "s185d03searchappsa"
  resource_group_name      = data.azurerm_resource_group.rg.name
  location                 = data.azurerm_resource_group.rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  tags                     = data.azurerm_resource_group.rg.tags
}

resource "azurerm_storage_container" "functionappsc" {
  name                  = "s185d03searchappsc"
  storage_account_name  = azurerm_storage_account.functionappsa.name
  container_access_type = "private"
}

resource "azurerm_service_plan" "functionappsp" {
  name                = "s185d03searchappsp"
  resource_group_name = data.azurerm_resource_group.rg.name
  location            = data.azurerm_resource_group.rg.location
  os_type             = "Linux"
  sku_name            = "B1"
  tags                = data.azurerm_resource_group.rg.tags
}

resource "azurerm_linux_function_app" "functionapp" {
  name                       = "s185d03searchappfa"
  resource_group_name        = data.azurerm_resource_group.rg.name
  location                   = data.azurerm_resource_group.rg.location
  service_plan_id            = azurerm_service_plan.functionappsp.id
  storage_account_name       = azurerm_storage_account.functionappsa.name
  storage_account_access_key = azurerm_storage_account.functionappsa.primary_access_key

  site_config {
    always_on = false
  }

  app_settings = {
    FUNCTIONS_WORKER_RUNTIME = "dotnet"
  }

  tags = data.azurerm_resource_group.rg.tags
}
