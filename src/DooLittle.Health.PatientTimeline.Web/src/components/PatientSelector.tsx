import React from 'react';
import { Autocomplete, TextField, Box, Typography } from '@mui/material';
import type { Patient } from '../types';

interface PatientSelectorProps {
  patients: Patient[];
  selectedPatientId: string | null;
  onPatientChange: (patientId: string | null) => void;
}

const PatientSelector: React.FC<PatientSelectorProps> = ({ patients, selectedPatientId, onPatientChange }) => {
  const selectedPatient = patients.find(p => p.syntheaId === selectedPatientId) || null;

  const getPatientDisplayName = (patient: Patient) => {
    const fullName = patient.firstName && patient.lastName 
      ? `${patient.firstName} ${patient.lastName}` 
      : patient.name || '';
    const patientId = patient.syntheaId || `ID: ${patient.id}`;
    return fullName ? `${fullName} (${patientId})` : patientId;
  };

  const calculateAge = (dateOfBirth: string) => {
    const birthDate = new Date(dateOfBirth);
    const today = new Date();
    let age = today.getFullYear() - birthDate.getFullYear();
    const monthDiff = today.getMonth() - birthDate.getMonth();
    
    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }
    
    return age;
  };

  const getGenderDisplay = (gender?: string) => {
    switch (gender?.toUpperCase()) {
      case 'M':
        return 'Male';
      case 'F':
        return 'Female';
      default:
        return gender || 'Unknown';
    }
  };

  return (
    <Box sx={{ mb: 3 }}>
      <Typography variant="h6" gutterBottom sx={{ fontWeight: 600, color: 'text.primary' }}>
        Patient Selection
      </Typography>
      <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
        Search and select a patient to view their healthcare timeline
      </Typography>
      <Autocomplete
        options={patients}
        getOptionLabel={getPatientDisplayName}
        value={selectedPatient}
        onChange={(_, newValue) => onPatientChange(newValue ? (newValue.syntheaId ?? null) : null)}
        renderInput={(params) => (
          <TextField
            {...params}
            label="Search Patients"
            placeholder="Type patient name or ID..."
            fullWidth
            variant="outlined"
          />
        )}
        renderOption={(props, option) => (
          <Box component="li" {...props} sx={{ py: 1.5, px: 2 }}>
            <Box>
              <Typography variant="body1" sx={{ fontWeight: 500 }}>
                {option.firstName} {option.lastName}
              </Typography>
              <Typography variant="body2" color="text.secondary">
                ID: {option.syntheaId || option.id} • Age: {calculateAge(option.dateOfBirth)} • Sex: {getGenderDisplay(option.gender)}
              </Typography>
            </Box>
          </Box>
        )}
        isOptionEqualToValue={(option, value) => option.syntheaId === value.syntheaId}
        noOptionsText="No patients found"
        clearOnEscape
        autoHighlight
        selectOnFocus
      />
    </Box>
  );
};

export default PatientSelector;