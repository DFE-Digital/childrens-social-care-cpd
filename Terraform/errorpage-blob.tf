data "azurerm_storage_account" "s185errorpage-sa" {
  name                     = "s185errorpage"
  resource_group_name      = "s185d01-childrens-social-care-shared-rg"
}

data "azurerm_storage_container" "s185errorpage-sc" {
  name                  = "s185errorpage"
  storage_account_name  = data.azurerm_storage_account.s185errorpage-sa.name
}

resource "azurerm_storage_blob" "error-403" {
  name                   = "403.html"
  storage_account_name   = data.azurerm_storage_account.s185errorpage-sa.name
  storage_container_name = data.azurerm_storage_container.s185errorpage-sc.name
  type                   = "Block"
  source                 = "./Error-pages/403.html"  
}

resource "azurerm_storage_blob" "error-502" {
  name                   = "502.html"
  storage_account_name   = data.azurerm_storage_account.s185errorpage-sa.name
  storage_container_name = data.azurerm_storage_container.s185errorpage-sc.name
  type                   = "Block"
  source                 = "./Error-pages/502.html"  
}

output "blob_502_url" {
  value = azurerm_storage_blob.error-502.url
}

output "blob_403_url" {
  value = azurerm_storage_blob.error-403.url
}

