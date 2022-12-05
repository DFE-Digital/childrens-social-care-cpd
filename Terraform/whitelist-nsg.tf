data "azurerm_key_vault" "kv" {
  name                = var.key_vault_name[terraform.workspace]
  resource_group_name = "s185d01-childrens-social-care-shared-rg"
}

data "azurerm_key_vault_secret" "ips" {
  name         = "dfe-ips"
  key_vault_id = data.azurerm_key_vault.kv.id
}

data "azurerm_key_vault_secret" "dev-ips" {
  name         = "dfe-dev-ips"
  key_vault_id = data.azurerm_key_vault.kv.id
}

locals {
  ips     = compact((split(" ", data.azurerm_key_vault_secret.ips.value)))
  dev-ips = compact((split(" ", data.azurerm_key_vault_secret.dev-ips.value)))
}

resource "azurerm_network_security_rule" "whitelist-rules" {
  count                       = length(local.ips)
  name                        = "Allow-WhiteList-${count.index}"
  priority                    = 1500 + count.index
  direction                   = "Inbound"
  access                      = "Allow"
  protocol                    = "Tcp"
  source_port_range           = "*"
  destination_port_range      = "*"
  source_address_prefix       = local.ips[count.index]
  destination_address_prefix  = "*"
  resource_group_name         = data.azurerm_resource_group.rg.name
  network_security_group_name = azurerm_network_security_group.nsg.name
}

resource "azurerm_network_security_rule" "dev-whitelist-rules" {
  count                       = length(local.dev-ips)
  name                        = "Allow-WhiteList-Dev-${count.index}"
  priority                    = 500 + count.index
  direction                   = "Inbound"
  access                      = "Allow"
  protocol                    = "Tcp"
  source_port_range           = "*"
  destination_port_range      = "*"
  source_address_prefix       = local.dev-ips[count.index]
  destination_address_prefix  = "*"
  resource_group_name         = data.azurerm_resource_group.rg.name
  network_security_group_name = azurerm_network_security_group.nsg.name
}
