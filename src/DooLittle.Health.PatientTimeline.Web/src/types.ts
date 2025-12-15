export interface Patient {
  id: number;
  name?: string;
  firstName?: string;
  lastName?: string;
  dateOfBirth: string;
  gender?: string;
  medicalRecordNumber?: string;
  syntheaId?: string;
  timelineEvents?: TimelineEvent[];
}

export interface TimelineEvent {
  id: number;
  patientId: number;
  title: string;
  description?: string;
  eventDate: string;
  eventType?: string;
  details?: string;
}

export interface FHIRTimelineData {
  // FHIR JSON structure - we'll parse this as needed
  [key: string]: any;
}