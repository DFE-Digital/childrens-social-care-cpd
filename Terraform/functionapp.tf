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

    CPD_SEARCH_API_KEY                    = var.cpd_search_api_key
    CPD_INSTRUMENTATION_CONNECTIONSTRING  = var.cpd_instrumentation_connectionstring
    VCS_TAG                               = var.vcs_tag
    CPD_SEARCH_BATCH_SIZE                 = var.cpd_search_batch_size
    CPD_SEARCH_ENDPOINT                   = var.cpd_search_endpoint
    CPD_SEARCH_INDEX_NAME                 = var.cpd_search_index_name
    CPD_DELIVERY_KEY                      = var.cpd_delivery_key
    CPD_CONTENTFUL_ENVIRONMENT            = var.cpd_contentful_env[terraform.workspace]
    CPD_SPACE_ID                          = var.cpd_space_id
    CPD_SEARCH_RECREATE_INDEX_ON_REBUILD  = var.cpd_search_recreate_index_on_rebuild
  }

  tags = data.azurerm_resource_group.rg.tags
}
