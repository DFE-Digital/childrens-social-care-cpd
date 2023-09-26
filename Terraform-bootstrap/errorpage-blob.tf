resource "azurerm_storage_account" "s185errorpage-sa" {
  name                     = "s185errorpage"
  resource_group_name      = var.key_vault_rg[terraform.workspace]
  account_replication_type = "LRS"
  account_tier             = "Standard"
  location                 = azurerm_resource_group.rg.location
}

resource "azurerm_storage_container" "s185errorpage-sc" {
  name                 = "s185errorpage"
  storage_account_name = azurerm_storage_account.s185errorpage-sa.name
}

resource "azurerm_storage_blob" "error-403" {
  name                   = "403.html"
  storage_account_name   = azurerm_storage_account.s185errorpage-sa.name
  storage_container_name = azurerm_storage_container.s185errorpage-sc.name
  type                   = "Block"
  source                 = "./Error-pages/403.html"
}

resource "azurerm_storage_blob" "error-502" {
  name                   = "502.html"
  storage_account_name   = azurerm_storage_account.s185errorpage-sa.name
  storage_container_name = azurerm_storage_container.s185errorpage-sc.name
  type                   = "Block"
  source                 = "./Error-pages/502.html"
}
