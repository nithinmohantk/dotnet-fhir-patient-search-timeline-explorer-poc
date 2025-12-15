#!/bin/bash

# DooLittle Health Patient Timeline - CSV Data Import Script
# This script imports CSV data into the database via the API
# Author: Nithin Mohan T K

set -e

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

# API endpoint
API_URL="http://localhost:8080/api/patients/import-csv"

print_info "Starting CSV data import..."
print_info "API URL: $API_URL"

# Make the API call to import CSV data
response=$(curl -s -o response.txt -w "%{http_code}" -X POST "$API_URL" \
  -H "Content-Type: application/json")

if [ "$response" -eq 200 ]; then
    print_success "CSV data imported successfully!"
    cat response.txt
else
    print_error "Failed to import CSV data. HTTP Status: $response"
    echo "Response:"
    cat response.txt
    exit 1
fi

# Clean up
rm -f response.txt

print_info "Verifying imported data..."

# Check if patients were imported
patients_response=$(curl -s "http://localhost:8080/api/patients")
patient_count=$(echo "$patients_response" | jq '. | length' 2>/dev/null || echo "0")

if [ "$patient_count" -gt 0 ]; then
    print_success "Found $patient_count patients in the database"
else
    print_warning "No patients found in the database"
fi

print_success "CSV import process completed!"