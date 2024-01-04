resource "azurerm_storage_account" "functionappsa" {
  name                     = var.functionapp_storage_account_name[terraform.workspace]
  resource_group_name      = data.azurerm_resource_group.rg.name
  location                 = data.azurerm_resource_group.rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  tags                     = data.azurerm_resource_group.rg.tags
}

resource "azurerm_storage_container" "functionappsc" {
  name                  = var.functionapp_storage_container_name[terraform.workspace]
  storage_account_name  = azurerm_storage_account.functionappsa.name
  container_access_type = "private"
}

resource "azurerm_service_plan" "functionappsp" {
  name                = var.functionapp_service_plan_name[terraform.workspace]
  resource_group_name = data.azurerm_resource_group.rg.name
  location            = data.azurerm_resource_group.rg.location
  os_type             = "Linux"
  sku_name            = var.functionapp_sku_name[terraform.workspace]
  worker_count        = var.functionapp_worker_count[terraform.workspace]
  tags                = data.azurerm_resource_group.rg.tags
}

resource "azurerm_linux_function_app" "functionapp" {
  name                       = var.functionapp_name[terraform.workspace]
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
