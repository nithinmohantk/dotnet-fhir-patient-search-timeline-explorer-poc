import React, { useState } from 'react';
import Plot from 'react-plotly.js';
import { Modal, Box, Typography, Chip, Divider, IconButton, Paper, Tabs, Tab } from '@mui/material';
import { Close } from '@mui/icons-material';
import type { FHIRTimelineData, Patient } from '../types';
import type { Data, Layout, Config } from 'plotly.js';
import VerticalTimeline from './VerticalTimeline';

interface PatientTimelineProps {
  timelineData: FHIRTimelineData | null;
  patientData?: Patient | null; // Add patient data with timeline events
}

interface TimelineEvent {
  id: string;
  title: string;
  date: string;
  type: string;
  description?: string;
  details?: string;
  resourceType: string;
  resource: any; // Full FHIR resource for modal details
}

interface TimelineEvent {
  id: string;
  title: string;
  date: string;
  type: string;
  description?: string;
  details?: string;
  resourceType: string;
  resource: any; // Full FHIR resource for modal details
}

const getEventIcon = (resourceType: string) => {
  switch (resourceType) {
    case 'Patient':
      return '👤';
    case 'Encounter':
      return '🏥';
    case 'Condition':
      return '⚕️';
    case 'Medication':
      return '💊';
    case 'Procedure':
      return '🔬';
    case 'Observation':
      return '📊';
    case 'Immunization':
      return '💉';
    default:
      return '🏥';
  }
};

const getEventColor = (resourceType: string): string => {
  switch (resourceType) {
    case 'Patient':
      return '#1976d2'; // Blue
    case 'Encounter':
      return '#388e3c'; // Green
    case 'Condition':
      return '#d32f2f'; // Red
    case 'Medication':
      return '#7b1fa2'; // Purple
    case 'Procedure':
      return '#f57c00'; // Orange
    case 'Observation':
      return '#1976d2'; // Blue
    case 'Immunization':
      return '#388e3c'; // Green
    default:
      return '#757575'; // Grey
  }
};

const getEventSymbol = (resourceType: string): string => {
  switch (resourceType) {
    case 'Patient':
      return 'circle';
    case 'Encounter':
      return 'diamond';
    case 'Condition':
      return 'triangle-up';
    case 'Medication':
      return 'square';
    case 'Procedure':
      return 'triangle-down';
    case 'Observation':
      return 'hexagon';
    case 'Immunization':
      return 'star';
    default:
      return 'circle';
  }
};

const parseFHIRData = (data: FHIRTimelineData): TimelineEvent[] => {
  if (!data || !data.entry) return [];

  const events: TimelineEvent[] = [];

  data.entry.forEach((entry: any) => {
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
      resource: resource, // Store full resource for modal
    };

    events.push(event);
  });

  return events.sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime());
};

const parseDatabaseEvents = (patient: Patient): TimelineEvent[] => {
  if (!patient.timelineEvents) return [];

  return patient.timelineEvents.map((event: any) => ({
    id: event.id.toString(),
    title: event.title,
    date: event.eventDate,
    type: event.eventType || 'Event',
    description: event.description,
    details: event.details,
    resourceType: mapEventTypeToResourceType(event.eventType),
    resource: event, // Store the event as resource for modal
  }));
};

const mapEventTypeToResourceType = (eventType?: string): string => {
  switch (eventType) {
    case 'Appointment':
      return 'Encounter';
    case 'Diagnosis':
      return 'Condition';
    case 'Medication':
      return 'Medication';
    case 'Wellness Visit':
      return 'Encounter';
    case 'Emergency Visit':
      return 'Encounter';
    case 'Encounter':
      return 'Encounter';
    default:
      return 'Encounter';
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
    default:
      return resource.resourceType;
  }
};

const getResourceDate = (resource: any): string => {
  // Try different date fields based on resource type
  const dateFields = ['effectiveDateTime', 'performedDateTime', 'onsetDateTime', 'recordedDate', 'date'];
  for (const field of dateFields) {
    if (resource[field]) return resource[field];
  }
  // For Patient, use birthDate
  if (resource.resourceType === 'Patient' && resource.birthDate) return resource.birthDate;
  // Fallback to current date if no date found
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

// Helper functions for detailed modal content
const renderEncounterDetails = (resource: any) => (
  <Box sx={{ mt: 2 }}>
    <Divider sx={{ my: 2 }} />
    <Typography variant="h6" sx={{ mb: 2, color: 'primary.main' }}>
      Encounter Details
    </Typography>
    {resource.type?.[0]?.text && (
      <Typography variant="body2" sx={{ mb: 1 }}>
        <strong>Type:</strong> {resource.type[0].text}
      </Typography>
    )}
    {resource.class?.display && (
      <Typography variant="body2" sx={{ mb: 1 }}>
        <strong>Class:</strong> {resource.class.display}
      </Typography>
    )}
    {resource.reasonCode?.[0]?.text && (
      <Typography variant="body2" sx={{ mb: 1 }}>
        <strong>Reason:</strong> {resource.reasonCode[0].text}
      </Typography>
    )}
    {resource.participant && (
      <Typography variant="body2" sx={{ mb: 1 }}>
        <strong>Participants:</strong> {resource.participant.map((p: any) => p.individual?.display).filter(Boolean).join(', ')}
      </Typography>
    )}
    {resource.location && (
      <Typography variant="body2" sx={{ mb: 1 }}>
        <strong>Location:</strong> {resource.location.map((l: any) => l.location?.display).filter(Boolean).join(', ')}
      </Typography>
    )}
  </Box>
);

const renderMedicationDetails = (resource: any) => (
  <Box sx={{ mt: 2 }}>
    <Divider sx={{ my: 2 }} />
    <Typography variant="h6" sx={{ mb: 2, color: 'primary.main' }}>
      Prescription Details
    </Typography>
    {resource.dosage?.[0] && (
      <>
        {resource.dosage[0].text && (
          <Typography variant="body2" sx={{ mb: 1 }}>
            <strong>Instructions:</strong> {resource.dosage[0].text}
          </Typography>
        )}
        {resource.dosage[0].route?.text && (
          <Typography variant="body2" sx={{ mb: 1 }}>
            <strong>Route:</strong> {resource.dosage[0].route.text}
          </Typography>
        )}
        {resource.dosage[0].doseAndRate?.[0]?.doseQuantity && (
          <Typography variant="body2" sx={{ mb: 1 }}>
            <strong>Dosage:</strong> {resource.dosage[0].doseAndRate[0].doseQuantity.value} {resource.dosage[0].doseAndRate[0].doseQuantity.unit}
          </Typography>
        )}
        {resource.dosage[0].timing?.repeat?.frequency && (
          <Typography variant="body2" sx={{ mb: 1 }}>
            <strong>Frequency:</strong> {resource.dosage[0].timing.repeat.frequency}x per {resource.dosage[0].timing.repeat.period} {resource.dosage[0].timing.repeat.periodUnit}
          </Typography>
        )}
      </>
    )}
  </Box>
);

const renderProcedureDetails = (resource: any) => (
  <Box sx={{ mt: 2 }}>
    <Divider sx={{ my: 2 }} />
    <Typography variant="h6" sx={{ mb: 2, color: 'primary.main' }}>
      Procedure Details
    </Typography>
    {resource.outcome?.text && (
      <Typography variant="body2" sx={{ mb: 1 }}>
        <strong>Outcome:</strong> {resource.outcome.text}
      </Typography>
    )}
    {resource.performedPeriod && (
      <Typography variant="body2" sx={{ mb: 1 }}>
        <strong>Performed:</strong> {resource.performedPeriod.start} to {resource.performedPeriod.end || 'ongoing'}
      </Typography>
    )}
    {resource.bodySite && (
      <Typography variant="body2" sx={{ mb: 1 }}>
        <strong>Body Site:</strong> {resource.bodySite.map((bs: any) => bs.text || bs.coding?.[0]?.display).filter(Boolean).join(', ')}
      </Typography>
    )}
  </Box>
);

const renderImmunizationDetails = (resource: any) => (
  <Box sx={{ mt: 2 }}>
    <Divider sx={{ my: 2 }} />
    <Typography variant="h6" sx={{ mb: 2, color: 'primary.main' }}>
      Immunization Details
    </Typography>
    {resource.vaccineCode?.text && (
      <Typography variant="body2" sx={{ mb: 1 }}>
        <strong>Vaccine:</strong> {resource.vaccineCode.text}
      </Typography>
    )}
    {resource.doseQuantity && (
      <Typography variant="body2" sx={{ mb: 1 }}>
        <strong>Dose:</strong> {resource.doseQuantity.value} {resource.doseQuantity.unit}
      </Typography>
    )}
    {resource.route?.text && (
      <Typography variant="body2" sx={{ mb: 1 }}>
        <strong>Route:</strong> {resource.route.text}
      </Typography>
    )}
    {resource.site?.text && (
      <Typography variant="body2" sx={{ mb: 1 }}>
        <strong>Site:</strong> {resource.site.text}
      </Typography>
    )}
  </Box>
);

const PatientTimeline: React.FC<PatientTimelineProps> = ({ timelineData, patientData }) => {
  const [selectedEvent, setSelectedEvent] = useState<TimelineEvent | null>(null);
  const [modalOpen, setModalOpen] = useState(false);
  const [activeTab, setActiveTab] = useState(3);

  if (!timelineData && !patientData) {
    return (
      <Box sx={{ textAlign: 'center', py: 4 }}>
        <Typography variant="h6" color="text.secondary">
          Loading patient timeline...
        </Typography>
      </Box>
    );
  }

  const events = timelineData ? parseFHIRData(timelineData) : parseDatabaseEvents(patientData!);

  if (events.length === 0) {
    return (
      <Box sx={{ textAlign: 'center', py: 4 }}>
        <Typography variant="h6" color="text.secondary">
          No timeline data found for this patient
        </Typography>
        <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
          FHIR data will appear here when available
        </Typography>
      </Box>
    );
  }

  const handlePointClick = (event: any) => {
    if (event.points && event.points.length > 0) {
      const point = event.points[0];
      const eventId = point.customdata;
      const selectedEventData = events.find(e => e.id === eventId);
      if (selectedEventData) {
        setSelectedEvent(selectedEventData);
        setModalOpen(true);
      }
    }
  };

  const renderSimpleTimeline = () => (
    <Box sx={{ maxHeight: '600px', overflow: 'auto' }}>
      {events.map((event) => (
        <Paper
          key={event.id}
          elevation={1}
          sx={{
            p: 2,
            mb: 2,
            cursor: 'pointer',
            '&:hover': { bgcolor: 'grey.50' },
            borderLeft: 4,
            borderColor: getEventColor(event.resourceType),
          }}
          onClick={() => {
            setSelectedEvent(event);
            setModalOpen(true);
          }}
        >
          <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
            <Typography variant="h6" sx={{ fontSize: '1.1rem', mr: 1 }}>
              {getEventIcon(event.resourceType)}
            </Typography>
            <Typography variant="h6" sx={{ flexGrow: 1 }}>
              {event.title}
            </Typography>
            <Chip
              label={event.type}
              color={getChipColor(event.resourceType)}
              size="small"
            />
          </Box>
          <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
            {new Date(event.date).toLocaleDateString('en-US', {
              weekday: 'long',
              year: 'numeric',
              month: 'long',
              day: 'numeric',
              ...(event.date.includes('T') && {
                hour: '2-digit',
                minute: '2-digit',
              }),
            })}
          </Typography>
          {event.description && (
            <Typography variant="body2" sx={{ color: 'text.primary' }}>
              {event.description}
            </Typography>
          )}
        </Paper>
      ))}
    </Box>
  );

  const renderLinkedTimeline = () => (
    <Box sx={{ maxHeight: '600px', overflow: 'auto', position: 'relative' }}>
      {events
        .sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime())
        .map((event, index) => (
          <Box
            key={event.id}
            sx={{
              display: 'flex',
              alignItems: 'flex-start',
              mb: index < events.length - 1 ? 4 : 0,
              position: 'relative',
            }}
          >
            {/* Timeline connector line */}
            {index < events.length - 1 && (
              <Box
                sx={{
                  position: 'absolute',
                  left: 20,
                  top: 40,
                  width: 2,
                  height: 60,
                  bgcolor: 'primary.main',
                  zIndex: 1,
                }}
              />
            )}

            {/* Timeline dot */}
            <Box
              sx={{
                width: 16,
                height: 16,
                borderRadius: '50%',
                bgcolor: getEventColor(event.resourceType),
                border: '3px solid white',
                boxShadow: '0 2px 4px rgba(0,0,0,0.2)',
                mr: 3,
                mt: 1,
                zIndex: 2,
                position: 'relative',
              }}
            />

            {/* Event card */}
            <Paper
              elevation={2}
              sx={{
                p: 2,
                flexGrow: 1,
                cursor: 'pointer',
                '&:hover': {
                  bgcolor: 'grey.50',
                  transform: 'translateY(-2px)',
                  boxShadow: '0 4px 8px rgba(0,0,0,0.15)',
                },
                transition: 'all 0.2s ease-in-out',
                borderLeft: `4px solid ${getEventColor(event.resourceType)}`,
              }}
              onClick={() => {
                setSelectedEvent(event);
                setModalOpen(true);
              }}
            >
              <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                <Typography variant="h6" sx={{ fontSize: '1.2rem', mr: 2 }}>
                  {getEventIcon(event.resourceType)}
                </Typography>
                <Typography variant="h6" sx={{ flexGrow: 1 }}>
                  {event.title}
                </Typography>
                <Chip
                  label={event.type}
                  color={getEventColor(event.resourceType) as any}
                  size="small"
                  sx={{ ml: 1 }}
                />
              </Box>

              <Typography
                variant="body2"
                color="text.secondary"
                sx={{ mb: 1, fontWeight: 500 }}
              >
                {new Date(event.date).toLocaleDateString('en-US', {
                  weekday: 'long',
                  year: 'numeric',
                  month: 'long',
                  day: 'numeric',
                  ...(event.date.includes('T') && {
                    hour: '2-digit',
                    minute: '2-digit',
                  }),
                })}
              </Typography>

              {event.description && (
                <Typography variant="body2" sx={{ color: 'text.primary', lineHeight: 1.5 }}>
                  {event.description}
                </Typography>
              )}

              {/* Connection indicator for related events */}
              {index < events.length - 1 && (
                <Box sx={{ mt: 2, display: 'flex', alignItems: 'center' }}>
                  <Typography variant="caption" color="text.secondary" sx={{ mr: 1 }}>
                    Next:
                  </Typography>
                  <Chip
                    label={events[index + 1].title}
                    size="small"
                    variant="outlined"
                    sx={{ fontSize: '0.7rem' }}
                  />
                </Box>
              )}
            </Paper>
          </Box>
        ))}
    </Box>
  );

  const renderInteractiveTimeline = () => {
    // Group events by type for different traces
    const eventTypes = [...new Set(events.map(e => e.resourceType))];

    const traces: Data[] = eventTypes.map(type => {
      const typeEvents = events.filter(e => e.resourceType === type);
      return {
        x: typeEvents.map(e => new Date(e.date)),
        y: typeEvents.map(e => e.resourceType), // Use resource type as y-axis
        mode: 'markers' as const,
        type: 'scatter' as const,
        name: type,
        marker: {
          symbol: getEventSymbol(type),
          size: 12,
          color: getEventColor(type),
        },
        text: typeEvents.map(e => `${getEventIcon(e.resourceType)} ${e.title}<br>${e.description || ''}`),
        hovertemplate:
          '<b>%{text}</b><br>' +
          'Date: %{x}<br>' +
          '<extra></extra>',
        customdata: typeEvents.map(e => e.id),
      };
    });

    const layout: Partial<Layout> = {
      title: {
        text: 'Patient Medical Timeline',
        font: { size: 24, color: '#1976d2' }
      },
      xaxis: {
        title: { text: 'Date' },
        type: 'date' as const,
        tickformat: '%Y-%m-%d',
        tickangle: -45,
      },
      yaxis: {
        title: { text: 'Event Type' },
        categoryorder: 'array' as const,
        categoryarray: ['Patient', 'Encounter', 'Condition', 'Medication', 'Procedure', 'Observation', 'Immunization'],
      },
      height: 600,
      margin: { l: 100, r: 50, t: 80, b: 100 },
      showlegend: true,
      legend: {
        orientation: 'h' as const,
        y: -0.2,
      },
    };

    const config: Partial<Config> = {
      displayModeBar: true,
      displaylogo: false,
      modeBarButtonsToRemove: ['pan2d', 'select2d', 'lasso2d', 'autoScale2d'] as const,
    };

    return (
      <Plot
        data={traces}
        layout={layout}
        config={config}
        onClick={handlePointClick}
        style={{ width: '100%', height: '600px' }}
      />
    );
  };

  const renderCombinedTimeline = () => (
    <Box sx={{ maxHeight: '600px', overflow: 'auto' }}>
      <Typography variant="h6" gutterBottom sx={{ color: 'primary.main', fontWeight: 600, mb: 3 }}>
        Patient Timeline
      </Typography>

      {events
        .sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime())
        .map((event, index) => (
          <Box
            key={event.id}
            sx={{
              display: 'flex',
              alignItems: 'center',
              py: 1.5,
              px: 2,
              borderBottom: index < events.length - 1 ? '1px solid' : 'none',
              borderColor: 'divider',
              '&:hover': {
                bgcolor: 'grey.50',
                cursor: 'pointer',
              },
              transition: 'background-color 0.2s ease',
            }}
            onClick={() => {
              setSelectedEvent(event);
              setModalOpen(true);
            }}
          >
            {/* Date */}
            <Typography
              variant="body2"
              sx={{
                minWidth: '120px',
                fontFamily: 'monospace',
                color: 'text.secondary',
                fontSize: '0.85rem',
              }}
            >
              {new Date(event.date).toLocaleDateString('en-US', {
                year: 'numeric',
                month: '2-digit',
                day: '2-digit',
                ...(event.date.includes('T') && {
                  hour: '2-digit',
                  minute: '2-digit',
                }),
              })}
            </Typography>

            {/* Patient/Event Info */}
            <Box sx={{ ml: 3, flexGrow: 1 }}>
              <Typography variant="body1" sx={{ fontWeight: 500 }}>
                {event.title}
              </Typography>
              {event.description && (
                <Typography variant="body2" color="text.secondary" sx={{ mt: 0.5 }}>
                  {event.description.length > 100
                    ? `${event.description.substring(0, 100)}...`
                    : event.description}
                </Typography>
              )}
            </Box>

            {/* Event Type Icon */}
            <Box sx={{ ml: 2 }}>
              <Typography variant="body2" sx={{ fontSize: '1.2rem' }}>
                {getEventIcon(event.resourceType)}
              </Typography>
            </Box>
          </Box>
        ))}

      {events.length === 0 && (
        <Box sx={{ textAlign: 'center', py: 4 }}>
          <Typography variant="body1" color="text.secondary">
            No timeline events found for this patient.
          </Typography>
        </Box>
      )}
    </Box>
  );

  return (
    <Box sx={{ mt: 3 }}>
      <Typography variant="h5" gutterBottom sx={{ mb: 3, color: 'primary.main', fontWeight: 600 }}>
        Patient Timeline
      </Typography>

      <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
        <Tabs value={activeTab} onChange={(_, newValue) => setActiveTab(newValue)} aria-label="timeline view tabs">
          <Tab label="Interactive Timeline" />
          <Tab label="Simple Timeline" />
          <Tab label="Linked Timeline" />
          <Tab label="Combined Timeline" />
          <Tab label="Vertical Timeline" />
        </Tabs>
      </Box>

      <Box sx={{ mt: 3 }}>
        {activeTab === 0 && renderInteractiveTimeline()}
        {activeTab === 1 && renderSimpleTimeline()}
        {activeTab === 2 && renderLinkedTimeline()}
        {activeTab === 3 && renderCombinedTimeline()}
        {activeTab === 4 && (
          <VerticalTimeline
            events={events}
            onEventClick={(event) => {
              setSelectedEvent(event);
              setModalOpen(true);
            }}
          />
        )}
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
                  {getEventIcon(selectedEvent.resourceType)} {selectedEvent.title}
                </Typography>
                <IconButton onClick={() => setModalOpen(false)} size="small">
                  <Close />
                </IconButton>
              </Box>

              <Chip
                label={selectedEvent.type}
                color={getChipColor(selectedEvent.resourceType)}
                size="small"
                sx={{ mb: 2 }}
              />

              <Typography variant="body2" color="text.secondary" sx={{ mb: 2, fontWeight: 500 }}>
                {new Date(selectedEvent.date).toLocaleDateString('en-US', {
                  weekday: 'long',
                  year: 'numeric',
                  month: 'long',
                  day: 'numeric',
                  ...(selectedEvent.date.includes('T') && {
                    hour: '2-digit',
                    minute: '2-digit',
                  }),
                })}
              </Typography>

              {selectedEvent.description && (
                <>
                  <Divider sx={{ my: 2 }} />
                  <Typography variant="body1" sx={{ mb: 2, lineHeight: 1.6 }}>
                    {selectedEvent.description}
                  </Typography>
                </>
              )}

              {selectedEvent.details && (
                <Box sx={{ mt: 2, p: 2, bgcolor: 'grey.50', borderRadius: 1 }}>
                  <Typography variant="body2" sx={{ fontWeight: 500, color: 'text.secondary', mb: 1 }}>
                    Clinical Details:
                  </Typography>
                  <Typography variant="body2" sx={{ lineHeight: 1.5, whiteSpace: 'pre-line' }}>
                    {selectedEvent.details}
                  </Typography>
                </Box>
              )}

              {/* Additional encounter and prescription details */}
              {selectedEvent.resourceType === 'Encounter' && renderEncounterDetails(selectedEvent.resource)}
              {selectedEvent.resourceType === 'Medication' && renderMedicationDetails(selectedEvent.resource)}
              {selectedEvent.resourceType === 'Procedure' && renderProcedureDetails(selectedEvent.resource)}
              {selectedEvent.resourceType === 'Immunization' && renderImmunizationDetails(selectedEvent.resource)}
            </>
          )}
        </Box>
      </Modal>
    </Box>
  );
};

export default PatientTimeline;