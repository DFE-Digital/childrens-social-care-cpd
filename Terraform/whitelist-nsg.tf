data "azurerm_key_vault" "kv" {
  name                = "s185d01-core-kv-01"
  resource_group_name = "s185d01-core"
}

data "azurerm_key_vault_secret" "ips" {
  name         = "dfe-ips"
  key_vault_id = data.azurerm_key_vault.kv.id
}

locals {
  ips = compact((split(" ", data.azurerm_key_vault_secret.ips.value)))
}

resource "azurerm_network_security_group" "whitelist-nsg" {
  name                = "s185d01-chidrens-social-care-cpd-whitelist-nsg"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  tags                = azurerm_resource_group.rg.tags
}

resource "azurerm_network_security_rule" "whitelist-rules" {
  count                       = length(local.ips)
  name                        = "Allow-WhiteList-${count.index}"
  priority                    = 500 + count.index
  direction                   = "Inbound"
  access                      = "Allow"
  protocol                    = "Tcp"
  source_port_range           = "*"
  destination_port_range      = "*"
  source_address_prefix       = local.ips[count.index]
  destination_address_prefix  = "*"
  resource_group_name         = azurerm_resource_group.rg.name
  network_security_group_name = azurerm_network_security_group.whitelist-nsg.name
}