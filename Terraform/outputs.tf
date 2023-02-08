# The public IP attached to the Application Gateway
output "public_ip_address" {
  value = data.azurerm_public_ip.pip1[*].ip_address
}
