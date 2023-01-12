#!/bin/bash
terraform workspace list
terraform workspace new Dev
terraform workspace new Test
terraform workspace new Pre-Prod
terraform workspace new Prod
terraform workspace new Load-Test
terraform workspace select Dev
terraform workspace list
