import React, { useState, useMemo } from 'react';
import {
  Box,
  Typography,
  Tabs,
  Tab,
  Card,
  CardContent,
  Chip,
  Avatar,
  Divider,
  Modal,
  IconButton,
  Paper
} from '@mui/material';
import { Close } from '@mui/icons-material';
import {
  User,
  Hospital,
  AlertTriangle,
  Pill,
  Microscope,
  BarChart3,
  Syringe,
  FileText
} from 'lucide-react';
import type { FHIRTimelineData } from '../types';

interface PatientExplorerProps {
  timelineData: FHIRTimelineData | null;
}

interface TimelineEvent {
  id: string;
  title: string;
  date: string;
  type: string;
  description?: string;
  details?: string;
  resourceType: string;
  resource: any;
}

const PatientExplorer: React.FC<PatientExplorerProps> = ({ timelineData }) => {
  const [activeTab, setActiveTab] = useState(0);
  const [selectedEvent, setSelectedEvent] = useState<TimelineEvent | null>(null);
  const [modalOpen, setModalOpen] = useState(false);

  const getEventIcon = (resourceType: string) => {
    switch (resourceType) {
      case 'Patient':
        return User;
      case 'Encounter':
        return Hospital;
      case 'Condition':
        return AlertTriangle;
      case 'Medication':
        return Pill;
      case 'Procedure':
        return Microscope;
      case 'Observation':
        return BarChart3;
      case 'Immunization':
        return Syringe;
      default:
        return FileText;
    }
  };

  const getResourceTitle = (resource: any): string => {
    switch (resource.resourceType) {
      case 'Patient':
        const name = resource.name?.[0];
        return name ? `${name.given?.join(' ')} ${name.family}` : 'Patient Record';
      case 'Encounter':
        return resource.type?.[0]?.text || resource.class?.display || 'Encounter';
      case 'Condition':
        return resource.code?.text || resource.code?.coding?.[0]?.display || 'Condition';
      case 'Medication':
        return resource.code?.text || resource.code?.coding?.[0]?.display || 'Medication';
      case 'Procedure':
        return resource.code?.text || resource.code?.coding?.[0]?.display || 'Procedure';
      case 'Observation':
        return resource.code?.text || resource.code?.coding?.[0]?.display || 'Observation';
      case 'Immunization':
        return resource.vaccineCode?.text || resource.vaccineCode?.coding?.[0]?.display || 'Immunization';
      default:
        return resource.resourceType;
    }
  };

  const getResourceDate = (resource: any): string => {
    const dateFields = ['effectiveDateTime', 'performedDateTime', 'onsetDateTime', 'recordedDate', 'date', 'occurrenceDateTime'];
    for (const field of dateFields) {
      if (resource[field]) return resource[field];
    }
    if (resource.resourceType === 'Patient' && resource.birthDate) return resource.birthDate;
    return new Date().toISOString();
  };

  const getResourceDescription = (resource: any): string => {
    switch (resource.resourceType) {
      case 'Patient':
        const birthDate = resource.birthDate ? new Date(resource.birthDate).toLocaleDateString() : '';
        const gender = resource.gender || '';
        return `Patient ${gender} ${birthDate ? `born ${birthDate}` : ''}`.trim();
      case 'Encounter':
        return resource.reasonCode?.[0]?.text || resource.type?.[0]?.text || 'Medical encounter';
      case 'Condition':
        return resource.severity?.text || resource.category?.[0]?.text || 'Medical condition';
      case 'Medication':
        return resource.dosage?.[0]?.text || 'Medication prescribed';
      case 'Procedure':
        return resource.outcome?.text || 'Medical procedure performed';
      case 'Observation':
        const value = resource.valueQuantity ? `${resource.valueQuantity.value} ${resource.valueQuantity.unit}` :
                     resource.valueString || resource.valueCodeableConcept?.text || '';
        return value ? `Result: ${value}` : 'Clinical observation';
      case 'Immunization':
        return resource.doseQuantity ? `Dose: ${resource.doseQuantity.value} ${resource.doseQuantity.unit}` : 'Immunization administered';
      default:
        return '';
    }
  };

  const getResourceDetails = (resource: any): string => {
    const details: string[] = [];

    switch (resource.resourceType) {
      case 'Patient':
        if (resource.address?.[0]) {
          const addr = resource.address[0];
          details.push(`Address: ${[addr.line, addr.city, addr.state, addr.postalCode].filter(Boolean).join(', ')}`);
        }
        if (resource.telecom) {
          resource.telecom.forEach((contact: any) => {
            if (contact.system === 'phone') details.push(`Phone: ${contact.value}`);
            if (contact.system === 'email') details.push(`Email: ${contact.value}`);
          });
        }
        break;
      case 'Encounter':
        if (resource.participant) {
          const participants = resource.participant.map((p: any) => p.individual?.display).filter(Boolean);
          if (participants.length) details.push(`Participants: ${participants.join(', ')}`);
        }
        if (resource.location) {
          const locations = resource.location.map((l: any) => l.location?.display).filter(Boolean);
          if (locations.length) details.push(`Location: ${locations.join(', ')}`);
        }
        break;
      case 'Condition':
        if (resource.bodySite) {
          const bodySites = resource.bodySite.map((bs: any) => bs.text || bs.coding?.[0]?.display).filter(Boolean);
          if (bodySites.length) details.push(`Body Site: ${bodySites.join(', ')}`);
        }
        if (resource.stage) {
          const stages = resource.stage.map((s: any) => s.summary?.text).filter(Boolean);
          if (stages.length) details.push(`Stage: ${stages.join(', ')}`);
        }
        break;
      case 'Medication':
        if (resource.dosage?.[0]?.route?.text) details.push(`Route: ${resource.dosage[0].route.text}`);
        if (resource.dosage?.[0]?.doseAndRate?.[0]?.doseQuantity) {
          const dose = resource.dosage[0].doseAndRate[0].doseQuantity;
          details.push(`Dosage: ${dose.value} ${dose.unit}`);
        }
        break;
      case 'Procedure':
        if (resource.performedPeriod) {
          const period = resource.performedPeriod;
          details.push(`Performed: ${period.start} to ${period.end || 'ongoing'}`);
        }
        break;
      case 'Observation':
        if (resource.referenceRange) {
          const range = resource.referenceRange[0];
          if (range.low || range.high) {
            const low = range.low ? `${range.low.value} ${range.low.unit}` : '';
            const high = range.high ? `${range.high.value} ${range.high.unit}` : '';
            details.push(`Reference Range: ${low} - ${high}`);
          }
        }
        if (resource.interpretation) {
          const interpretation = resource.interpretation[0]?.text || resource.interpretation[0]?.coding?.[0]?.display;
          if (interpretation) details.push(`Interpretation: ${interpretation}`);
        }
        break;
      case 'Immunization':
        if (resource.lotNumber) details.push(`Lot Number: ${resource.lotNumber}`);
        if (resource.expirationDate) details.push(`Expiration: ${resource.expirationDate}`);
        break;
    }

    return details.join('\n');
  };

  const getChipColor = (resourceType: string): "primary" | "error" | "info" | "success" | "default" => {
    switch (resourceType) {
      case 'Patient':
        return 'primary';
      case 'Encounter':
        return 'success';
      case 'Condition':
        return 'error';
      case 'Medication':
        return 'info';
      case 'Procedure':
        return 'primary';
      case 'Observation':
        return 'info';
      case 'Immunization':
        return 'success';
      default:
        return 'default';
    }
  };

  const events = useMemo(() => {
    if (!timelineData || !timelineData.entry) return [];

    const events: TimelineEvent[] = [];

    timelineData.entry.forEach((entry: any) => {
      const resource = entry.resource;
      if (!resource) return;

      const event: TimelineEvent = {
        id: resource.id || `${resource.resourceType}-${Math.random()}`,
        resourceType: resource.resourceType,
        title: getResourceTitle(resource),
        date: getResourceDate(resource),
        type: resource.resourceType,
        description: getResourceDescription(resource),
        details: getResourceDetails(resource),
        resource: resource,
      };

      events.push(event);
    });

    return events.sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime());
  }, [timelineData]);

  const groupedEvents = useMemo(() => {
    const groups: { [key: string]: TimelineEvent[] } = {
      overview: [],
      medications: [],
      procedures: [],
      conditions: [],
      observations: [],
      encounters: [],
      immunizations: [],
      other: []
    };

    events.forEach(event => {
      switch (event.resourceType) {
        case 'Patient':
          groups.overview.push(event);
          break;
        case 'Medication':
          groups.medications.push(event);
          break;
        case 'Procedure':
          groups.procedures.push(event);
          break;
        case 'Condition':
          groups.conditions.push(event);
          break;
        case 'Observation':
          groups.observations.push(event);
          break;
        case 'Encounter':
          groups.encounters.push(event);
          break;
        case 'Immunization':
          groups.immunizations.push(event);
          break;
        default:
          groups.other.push(event);
      }
    });

    return groups;
  }, [events]);

  const tabs = [
    { label: 'Patient Overview', key: 'overview', icon: User, color: 'primary' },
    { label: 'Prescriptions', key: 'medications', icon: Pill, color: 'info' },
    { label: 'Procedures', key: 'procedures', icon: Microscope, color: 'primary' },
    { label: 'Conditions', key: 'conditions', icon: AlertTriangle, color: 'error' },
    { label: 'Observations', key: 'observations', icon: BarChart3, color: 'info' },
    { label: 'Encounters', key: 'encounters', icon: Hospital, color: 'success' },
    { label: 'Immunizations', key: 'immunizations', icon: Syringe, color: 'success' },
  ];

  const renderEventCard = (event: TimelineEvent) => {
    const IconComponent = getEventIcon(event.resourceType);

    return (
      <Box
        key={event.id}
        sx={{
          width: { xs: '100%', sm: '48%', md: '31%' },
          mb: 2,
        }}
      >
        <Card
          sx={{
            height: '100%',
            cursor: 'pointer',
            transition: 'all 0.3s ease',
            '&:hover': {
              transform: 'translateY(-4px)',
              boxShadow: 4,
            },
          }}
          onClick={() => {
            setSelectedEvent(event);
            setModalOpen(true);
          }}
        >
          <CardContent>
            <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
              <Avatar sx={{ bgcolor: 'primary.main', mr: 2 }}>
                <IconComponent size={20} />
              </Avatar>
              <Box sx={{ flexGrow: 1 }}>
                <Typography variant="h6" component="div" sx={{ fontSize: '1rem', fontWeight: 600 }}>
                  {event.title}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  {new Date(event.date).toLocaleDateString()}
                </Typography>
              </Box>
            </Box>

            {event.description && (
              <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                {event.description.length > 100
                  ? `${event.description.substring(0, 100)}...`
                  : event.description}
              </Typography>
            )}

            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
              <Chip
                label={event.resourceType}
                size="small"
                color={getChipColor(event.resourceType)}
                variant="outlined"
              />
              <Typography variant="caption" color="text.secondary">
                {event.id}
              </Typography>
            </Box>
          </CardContent>
        </Card>
      </Box>
    );
  };

  const renderOverviewTab = () => {
    const patientEvent = groupedEvents.overview.find(e => e.resourceType === 'Patient');
    const recentEncounters = groupedEvents.encounters.slice(0, 5);
    const activeConditions = groupedEvents.conditions.filter(c =>
      !c.resource.clinicalStatus || c.resource.clinicalStatus.coding?.[0]?.code !== 'resolved'
    ).slice(0, 5);
    const currentMedications = groupedEvents.medications.slice(0, 5);

    return (
      <Box>
        <Typography variant="h5" gutterBottom sx={{ color: 'primary.main', mb: 3 }}>
          Patient Overview
        </Typography>

        {patientEvent && (
          <Paper sx={{ p: 3, mb: 3, bgcolor: 'primary.light', color: 'white' }}>
            <Typography variant="h6" gutterBottom>
              Basic Information
            </Typography>
            <Typography variant="body1" sx={{ mb: 1 }}>
              <strong>Name:</strong> {patientEvent.title}
            </Typography>
            <Typography variant="body1" sx={{ mb: 1 }}>
              <strong>DOB:</strong> {new Date(patientEvent.date).toLocaleDateString()}
            </Typography>
            <Typography variant="body1">
              <strong>Description:</strong> {patientEvent.description}
            </Typography>
          </Paper>
        )}

        <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 3 }}>
          <Box sx={{ flex: { xs: '1 1 100%', md: '1 1 30%' } }}>
            <Paper sx={{ p: 3, height: '100%' }}>
              <Typography variant="h6" gutterBottom sx={{ color: 'success.main' }}>
                Recent Encounters
              </Typography>
              {recentEncounters.length > 0 ? (
                recentEncounters.map(encounter => (
                  <Box key={encounter.id} sx={{ mb: 2, pb: 2, borderBottom: '1px solid', borderColor: 'divider' }}>
                    <Typography variant="body2" sx={{ fontWeight: 600 }}>
                      {encounter.title}
                    </Typography>
                    <Typography variant="caption" color="text.secondary">
                      {new Date(encounter.date).toLocaleDateString()}
                    </Typography>
                  </Box>
                ))
              ) : (
                <Typography variant="body2" color="text.secondary">
                  No recent encounters
                </Typography>
              )}
            </Paper>
          </Box>

          <Box sx={{ flex: { xs: '1 1 100%', md: '1 1 30%' } }}>
            <Paper sx={{ p: 3, height: '100%' }}>
              <Typography variant="h6" gutterBottom sx={{ color: 'error.main' }}>
                Active Conditions
              </Typography>
              {activeConditions.length > 0 ? (
                activeConditions.map(condition => (
                  <Box key={condition.id} sx={{ mb: 2, pb: 2, borderBottom: '1px solid', borderColor: 'divider' }}>
                    <Typography variant="body2" sx={{ fontWeight: 600 }}>
                      {condition.title}
                    </Typography>
                    <Typography variant="caption" color="text.secondary">
                      {new Date(condition.date).toLocaleDateString()}
                    </Typography>
                  </Box>
                ))
              ) : (
                <Typography variant="body2" color="text.secondary">
                  No active conditions
                </Typography>
              )}
            </Paper>
          </Box>

          <Box sx={{ flex: { xs: '1 1 100%', md: '1 1 30%' } }}>
            <Paper sx={{ p: 3, height: '100%' }}>
              <Typography variant="h6" gutterBottom sx={{ color: 'info.main' }}>
                Current Medications
              </Typography>
              {currentMedications.length > 0 ? (
                currentMedications.map(medication => (
                  <Box key={medication.id} sx={{ mb: 2, pb: 2, borderBottom: '1px solid', borderColor: 'divider' }}>
                    <Typography variant="body2" sx={{ fontWeight: 600 }}>
                      {medication.title}
                    </Typography>
                    <Typography variant="caption" color="text.secondary">
                      {new Date(medication.date).toLocaleDateString()}
                    </Typography>
                  </Box>
                ))
              ) : (
                <Typography variant="body2" color="text.secondary">
                  No current medications
                </Typography>
              )}
            </Paper>
          </Box>
        </Box>
      </Box>
    );
  };

  const renderTabContent = (tabKey: string) => {
    const tabEvents = groupedEvents[tabKey];

    if (tabKey === 'overview') {
      return renderOverviewTab();
    }

    return (
      <Box>
        <Typography variant="h5" gutterBottom sx={{ color: 'primary.main', mb: 3 }}>
          {tabs.find(tab => tab.key === tabKey)?.label}
        </Typography>

        {tabEvents.length > 0 ? (
          <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 2 }}>
            {tabEvents.map(event => renderEventCard(event))}
          </Box>
        ) : (
          <Box sx={{ textAlign: 'center', py: 6 }}>
            <Typography variant="h6" color="text.secondary" gutterBottom>
              No {tabKey} found
            </Typography>
            <Typography variant="body1" color="text.secondary">
              This patient has no recorded {tabKey}.
            </Typography>
          </Box>
        )}
      </Box>
    );
  };

  if (!events.length) {
    return (
      <Box sx={{ textAlign: 'center', py: 6 }}>
        <Typography variant="h6" color="text.secondary">
          No patient data available
        </Typography>
      </Box>
    );
  }

  return (
    <Box sx={{ width: '100%' }}>
      <Box sx={{ borderBottom: 1, borderColor: 'divider', mb: 3 }}>
        <Tabs
          value={activeTab}
          onChange={(_, newValue) => setActiveTab(newValue)}
          aria-label="patient explorer tabs"
          variant="scrollable"
          scrollButtons="auto"
        >
          {tabs.map((tab, _index) => {
            const IconComponent = tab.icon;
            const count = groupedEvents[tab.key].length;
            return (
              <Tab
                key={tab.key}
                label={
                  <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                    <IconComponent size={16} />
                    <span>{tab.label}</span>
                    {count > 0 && (
                      <Chip
                        label={count}
                        size="small"
                        color={tab.color as any}
                        sx={{ height: 18, fontSize: '0.7rem' }}
                      />
                    )}
                  </Box>
                }
                sx={{ minHeight: 48 }}
              />
            );
          })}
        </Tabs>
      </Box>

      <Box sx={{ mt: 3 }}>
        {renderTabContent(tabs[activeTab].key)}
      </Box>

      {/* Modal for event details */}
      <Modal
        open={modalOpen}
        onClose={() => setModalOpen(false)}
        aria-labelledby="event-modal-title"
        aria-describedby="event-modal-description"
      >
        <Box sx={{
          position: 'absolute',
          top: '50%',
          left: '50%',
          transform: 'translate(-50%, -50%)',
          width: { xs: '90%', sm: '80%', md: '600px' },
          maxHeight: '80vh',
          overflow: 'auto',
          bgcolor: 'background.paper',
          boxShadow: 24,
          p: 4,
          borderRadius: 2,
        }}>
          {selectedEvent && (
            <>
              <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                <Typography id="event-modal-title" variant="h6" component="h2" sx={{ fontWeight: 600 }}>
                  {selectedEvent.title}
                </Typography>
                <IconButton onClick={() => setModalOpen(false)}>
                  <Close />
                </IconButton>
              </Box>

              <Divider sx={{ my: 2 }} />

              <Typography variant="body1" sx={{ mb: 2 }}>
                {selectedEvent.description}
              </Typography>

              {selectedEvent.details && (
                <Box sx={{ mt: 2 }}>
                  <Typography variant="h6" sx={{ mb: 1, color: 'primary.main' }}>
                    Details
                  </Typography>
                  <Typography variant="body2" sx={{ whiteSpace: 'pre-wrap' }}>
                    {selectedEvent.details}
                  </Typography>
                </Box>
              )}

              <Box sx={{ mt: 3, pt: 2, borderTop: '1px solid', borderColor: 'divider' }}>
                <Typography variant="caption" color="text.secondary">
                  Resource Type: {selectedEvent.resourceType} | ID: {selectedEvent.id}
                </Typography>
              </Box>
            </>
          )}
        </Box>
      </Modal>
    </Box>
  );
};

export default PatientExplorer;