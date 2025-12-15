#!/bin/bash

# DooLittle Health Patient Timeline - Build Script
# This script provides common build and deployment operations
# Author: Nithin Mohan T K (Example project for experimentation)

set -e

PROJECT_NAME="DooLittle.Health.PatientTimeline"
API_PROJECT="src/DooLittle.Health.PatientTimeline.Api"
WEB_PROJECT="src/DooLittle.Health.PatientTimeline.Web"

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Function to print colored output
print_info() {
    echo -e "${BLUE}[INFO]${NC} $1"
}

print_success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# Function to show usage
usage() {
    echo "Usage: $0 [COMMAND]"
    echo ""
    echo "Commands:"
    echo "  build         Build all projects"
    echo "  test          Run tests (if any)"
    echo "  docker-build  Build Docker images"
    echo "  docker-run    Run application with Docker Compose"
    echo "  k8s-deploy    Deploy to Kubernetes"
    echo "  clean         Clean build artifacts"
    echo "  help          Show this help message"
    echo ""
}

# Function to build .NET projects
build_dotnet() {
    print_info "Building .NET API project..."
    cd "$API_PROJECT"
    dotnet restore
    dotnet build --configuration Release
    cd ../..
    print_success ".NET API project built successfully"
}

# Function to build React project
build_react() {
    print_info "Building React frontend..."
    cd "$WEB_PROJECT"
    npm install
    npm run build
    cd ../..
    print_success "React frontend built successfully"
}

# Function to build all projects
build_all() {
    print_info "Building all projects..."
    build_dotnet
    build_react
    print_success "All projects built successfully"
}

# Function to build Docker images
docker_build() {
    print_info "Building Docker images..."
    docker build -f deployments/docker/Dockerfile.api -t doolittlehealth/patient-timeline-api:latest .
    docker build -f deployments/docker/Dockerfile.web -t doolittlehealth/patient-timeline-web:latest .
    print_success "Docker images built successfully"
}

# Function to run with Docker Compose
docker_run() {
    print_info "Starting application with Docker Compose..."
    docker-compose up --build
}

# Function to deploy to Kubernetes
k8s_deploy() {
    print_info "Deploying to Kubernetes..."
    kubectl apply -f deployments/k8s/namespace.yaml
    kubectl apply -f deployments/k8s/
    print_success "Application deployed to Kubernetes"
}

# Function to clean build artifacts
clean() {
    print_info "Cleaning build artifacts..."
    rm -rf "$API_PROJECT/bin" "$API_PROJECT/obj" "$WEB_PROJECT/dist" "$WEB_PROJECT/node_modules"
    print_success "Build artifacts cleaned"
}

# Main script logic
case "${1:-help}" in
    build)
        build_all
        ;;
    test)
        print_warning "No tests implemented yet"
        ;;
    docker-build)
        docker_build
        ;;
    docker-run)
        docker_run
        ;;
    k8s-deploy)
        k8s_deploy
        ;;
    clean)
        clean
        ;;
    help|*)
        usage
        ;;
esac