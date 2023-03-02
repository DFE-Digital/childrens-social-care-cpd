data "azurerm_key_vault" "kv" {
  name                = var.key_vault_name[terraform.workspace]
  resource_group_name = var.key_vault_rg[terraform.workspace]
}

data "azurerm_key_vault_secret" "ips" {
  name         = "dfe-ips"
  key_vault_id = data.azurerm_key_vault.kv.id
}

data "azurerm_key_vault_secret" "dev-ips" {
  name         = "dfe-dev-ips"
  key_vault_id = data.azurerm_key_vault.kv.id
}
