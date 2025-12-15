import { useState, useEffect } from 'react';
import { ThemeProvider, createTheme } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import { Container, Typography, Box, AppBar, Toolbar, Avatar } from '@mui/material';
import LocalHospitalIcon from '@mui/icons-material/LocalHospital';
import PatientSelector from './components/PatientSelector';
import PatientTimeline from './components/PatientTimeline';
import { getPatients, getPatientTimeline } from './api';
import type { Patient, FHIRTimelineData } from './types';

const theme = createTheme({
  palette: {
    primary: {
      main: '#0066cc', // Professional blue
    },
    secondary: {
      main: '#00a86b', // Medical green
    },
    background: {
      default: '#f8f9fa', // Clean light background
      paper: '#ffffff',
    },
    text: {
      primary: '#2c3e50', // Dark professional text
      secondary: '#607d8b',
    },
  },
  typography: {
    fontFamily: '"Inter", "Roboto", "Helvetica", "Arial", sans-serif',
    h4: {
      fontWeight: 700,
      color: '#0066cc',
      letterSpacing: '-0.02em',
    },
    h6: {
      fontWeight: 600,
      color: '#2c3e50',
    },
  },
  components: {
    MuiAppBar: {
      styleOverrides: {
        root: {
          backgroundColor: '#ffffff',
          color: '#2c3e50',
          boxShadow: '0 2px 4px rgba(0,0,0,0.1)',
        },
      },
    },
    MuiPaper: {
      styleOverrides: {
        root: {
          borderRadius: 16,
          boxShadow: '0 4px 20px rgba(0, 0, 0, 0.08)',
        },
      },
    },
    MuiTextField: {
      styleOverrides: {
        root: {
          '& .MuiOutlinedInput-root': {
            borderRadius: 12,
            backgroundColor: '#fafbfc',
            '&:hover': {
              backgroundColor: '#f1f3f4',
            },
            '&.Mui-focused': {
              backgroundColor: '#ffffff',
            },
          },
        },
      },
    },
    MuiButton: {
      styleOverrides: {
        root: {
          borderRadius: 12,
          textTransform: 'none',
          fontWeight: 600,
        },
      },
    },
  },
});

function App() {
  const [patients, setPatients] = useState<Patient[]>([]);
  const [selectedPatientId, setSelectedPatientId] = useState<string | null>(null);
  const [timelineData, setTimelineData] = useState<FHIRTimelineData | null>(null);
  const [loading, setLoading] = useState(true);
  const [timelineLoading, setTimelineLoading] = useState(false);

  useEffect(() => {
    const fetchPatients = async () => {
      try {
        const data = await getPatients();
        setPatients(data);
      } catch (error) {
        console.error('Error fetching patients:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchPatients();
  }, []);

  useEffect(() => {
    const fetchTimelineData = async () => {
      if (selectedPatientId) {
        setTimelineLoading(true);
        try {
          const data = await getPatientTimeline(selectedPatientId);
          setTimelineData(data);
        } catch (error) {
          console.error('Error fetching timeline data:', error);
          setTimelineData(null);
        } finally {
          setTimelineLoading(false);
        }
      } else {
        setTimelineData(null);
      }
    };

    fetchTimelineData();
  }, [selectedPatientId]);

  const selectedPatient = patients.find(p => p.syntheaId === selectedPatientId);

  if (loading) {
    return (
      <ThemeProvider theme={theme}>
        <CssBaseline />
        <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
          <Typography variant="h6">Loading Patient Timeline Explorer...</Typography>
        </Box>
      </ThemeProvider>
    );
  }

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <AppBar position="static" elevation={0}>
        <Toolbar>
          <Avatar sx={{ bgcolor: 'primary.main', mr: 2 }}>
            <LocalHospitalIcon />
          </Avatar>
          <Box>
            <Typography variant="h6" component="div" sx={{ fontWeight: 700, lineHeight: 1.2 }}>
              DooLittle Health
            </Typography>
            <Typography variant="caption" sx={{ color: 'text.secondary', lineHeight: 1 }}>
              Patient Timeline Explorer
            </Typography>
          </Box>
        </Toolbar>
      </AppBar>

      <Container maxWidth="xl" sx={{ py: 4 }}>
        <Box sx={{ mb: 4 }}>
          <Typography variant="h4" component="h1" gutterBottom sx={{ mb: 1 }}>
            Patient Timeline Explorer
          </Typography>
          <Typography variant="body1" color="text.secondary">
            Select a patient to explore their comprehensive healthcare timeline powered by FHIR data.
          </Typography>
        </Box>

        <Box sx={{ p: 4, backgroundColor: 'background.paper', borderRadius: 3, mb: 3 }}>
          <PatientSelector
            patients={patients}
            selectedPatientId={selectedPatientId}
            onPatientChange={setSelectedPatientId}
          />
        </Box>

        {selectedPatient && (
          <Box sx={{ p: 4, backgroundColor: 'background.paper', borderRadius: 3 }}>
            {timelineLoading ? (
              <Box sx={{ display: 'flex', justifyContent: 'center', py: 4 }}>
                <Typography variant="h6" color="text.secondary">
                  Loading patient timeline...
                </Typography>
              </Box>
            ) : (timelineData || selectedPatient) ? (
              <PatientTimeline
                timelineData={timelineData}
                patientData={selectedPatient}
              />
            ) : (
              <Box sx={{ textAlign: 'center', py: 4 }}>
                <Typography variant="h6" color="text.secondary" gutterBottom>
                  Unable to load timeline data
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  The FHIR data for this patient could not be retrieved. Please try selecting another patient.
                </Typography>
              </Box>
            )}
          </Box>
        )}

        {!selectedPatient && (
          <Box sx={{ p: 6, backgroundColor: 'background.paper', borderRadius: 3, textAlign: 'center' }}>
            <LocalHospitalIcon sx={{ fontSize: 64, color: 'primary.main', mb: 2, opacity: 0.5 }} />
            <Typography variant="h5" color="text.secondary" gutterBottom>
              Select a Patient
            </Typography>
            <Typography variant="body1" color="text.secondary">
              Choose a patient from the dropdown above to view their comprehensive healthcare timeline.
            </Typography>
          </Box>
        )}
      </Container>
    </ThemeProvider>
  );
}

export default App;
