# Azure Infrastructure Deployment

This directory contains Terraform configurations to deploy the Patient Timeline application to Azure.

> **Note**: This is an example infrastructure configuration built by Nithin Mohan T K for experimentation and learning purposes.

## Prerequisites

- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) installed and authenticated
- [Terraform](https://www.terraform.io/downloads.html) >= 1.0.0 installed
- Azure subscription with appropriate permissions

## Quick Start

1. **Authenticate with Azure:**

   ```bash
   az login
   az account set --subscription "YOUR_SUBSCRIPTION_ID"
   ```

2. **Initialize Terraform:**

   ```bash
   cd deployments/terraform
   terraform init
   ```

3. **Configure variables:**

   ```bash
   cp terraform.tfvars.example terraform.tfvars
   # Edit terraform.tfvars with your values
   ```

4. **Plan deployment:**

   ```bash
   terraform plan -out=tfplan
   ```

5. **Apply deployment:**

   ```bash
   terraform apply tfplan
   ```

## Architecture

The Terraform configuration creates:

- **Resource Group**: Contains all resources
- **Azure Container Registry (ACR)**: For storing Docker images
- **Azure Kubernetes Service (AKS)**: Managed Kubernetes cluster
- **PostgreSQL Flexible Server**: Production database
- **Virtual Network & Subnet**: Network isolation
- **Role Assignments**: AKS can pull images from ACR

## Configuration

### Required Variables

| Variable | Description | Default |
|----------|-------------|---------|
| `db_admin_password` | PostgreSQL admin password | - |

### Optional Variables

| Variable | Description | Default |
|----------|-------------|---------|
| `environment` | Environment (dev/staging/prod) | dev |
| `location` | Azure region | East US |
| `unique_suffix` | Unique suffix for resource names | 001 |
| `node_count` | Initial AKS node count | 2 |
| `vm_size` | AKS node VM size | Standard_B2s |

## Outputs

After deployment, Terraform will output:

- Resource group name
- ACR login server and credentials
- AKS cluster name and kubeconfig
- PostgreSQL server details

## Deploy Application

After infrastructure is deployed:

1. **Build and push Docker images:**
   ```bash
   # Get ACR login server from Terraform output
   ACR_LOGIN_SERVER=$(terraform output -raw acr_login_server)

   # Login to ACR
   az acr login --name $(terraform output -raw acr_login_server | cut -d. -f1)

   # Build and push images
   docker build -f ../docker/Dockerfile.api -t $ACR_LOGIN_SERVER/patient-timeline-api:latest ../..
   docker build -f ../docker/Dockerfile.web -t $ACR_LOGIN_SERVER/patient-timeline-web:latest ../..
   docker push $ACR_LOGIN_SERVER/patient-timeline-api:latest
   docker push $ACR_LOGIN_SERVER/patient-timeline-web:latest
   ```

2. **Deploy to AKS:**
   ```bash
   # Get AKS credentials
   az aks get-credentials --resource-group $(terraform output -raw resource_group_name) --name $(terraform output -raw aks_cluster_name)

   # Update Kubernetes manifests with your ACR login server
   sed -i "s|your-acr-login-server|$ACR_LOGIN_SERVER|g" ../k8s/*.yaml

   # Deploy application
   kubectl apply -f ../k8s/
   ```

## Cleanup

To destroy all resources:

```bash
terraform destroy
```

## Cost Estimation

This configuration creates resources that may incur costs:

- AKS: ~$0.10-0.20/hour per node
- PostgreSQL: ~$0.50-1.00/hour
- ACR: ~$0.50/month
- Network: Minimal costs

Use Azure Pricing Calculator for detailed estimates.
