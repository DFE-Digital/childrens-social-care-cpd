# Locals used within the security rules
locals {
  ips = compact(split(" ", var.whitelist_ips))
}

# Network security rules for the DfE IP addresses
resource "azurerm_network_security_rule" "whitelist-rules" {
  count                       = length(local.ips)
  name                        = "GitHub-WhiteList-${count.index}"
  priority                    = 1000 + count.index
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

