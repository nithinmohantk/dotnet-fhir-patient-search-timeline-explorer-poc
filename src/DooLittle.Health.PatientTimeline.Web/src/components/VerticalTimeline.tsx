import React from 'react';
import {
  User,
  Hospital,
  AlertTriangle,
  Pill,
  Microscope,
  BarChart3,
  Syringe,
  Calendar,
  Clock,
  FileText,
  Activity,
  Stethoscope,
  Droplet,
  Zap,
  Shield,
  Eye,
  TestTube
} from 'lucide-react';

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

interface VerticalTimelineProps {
  events: TimelineEvent[];
  onEventClick?: (event: TimelineEvent) => void;
}

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
    case 'AllergyIntolerance':
      return Shield;
    case 'CarePlan':
      return FileText;
    case 'DiagnosticReport':
      return TestTube;
    case 'Device':
      return Zap;
    case 'ImagingStudy':
      return Eye;
    case 'MedicationRequest':
      return Pill;
    case 'MedicationAdministration':
      return Syringe;
    case 'Specimen':
      return Droplet;
    case 'Vital Signs':
      return Activity;
    case 'Laboratory':
      return TestTube;
    default:
      return FileText;
  }
};

const getEventGradient = (resourceType: string): string => {
  switch (resourceType) {
    case 'Patient':
      return 'from-blue-500 to-blue-600';
    case 'Encounter':
      return 'from-green-500 to-green-600';
    case 'Condition':
      return 'from-red-500 to-red-600';
    case 'Medication':
      return 'from-purple-500 to-purple-600';
    case 'Procedure':
      return 'from-orange-500 to-orange-600';
    case 'Observation':
      return 'from-cyan-500 to-cyan-600';
    case 'Immunization':
      return 'from-indigo-500 to-indigo-600';
    case 'AllergyIntolerance':
      return 'from-yellow-500 to-yellow-600';
    case 'CarePlan':
      return 'from-teal-500 to-teal-600';
    case 'DiagnosticReport':
      return 'from-pink-500 to-pink-600';
    case 'Device':
      return 'from-gray-500 to-gray-600';
    case 'ImagingStudy':
      return 'from-violet-500 to-violet-600';
    case 'MedicationRequest':
      return 'from-purple-500 to-purple-600';
    case 'MedicationAdministration':
      return 'from-indigo-500 to-indigo-600';
    case 'Specimen':
      return 'from-blue-500 to-blue-600';
    case 'Vital Signs':
      return 'from-red-500 to-red-600';
    case 'Laboratory':
      return 'from-green-500 to-green-600';
    default:
      return 'from-gray-500 to-gray-600';
  }
};

const getBadgeColor = (resourceType: string): string => {
  switch (resourceType) {
    case 'Patient':
      return 'bg-blue-100 text-blue-800';
    case 'Encounter':
      return 'bg-green-100 text-green-800';
    case 'Condition':
      return 'bg-red-100 text-red-800';
    case 'Medication':
      return 'bg-purple-100 text-purple-800';
    case 'Procedure':
      return 'bg-orange-100 text-orange-800';
    case 'Observation':
      return 'bg-cyan-100 text-cyan-800';
    case 'Immunization':
      return 'bg-indigo-100 text-indigo-800';
    case 'AllergyIntolerance':
      return 'bg-yellow-100 text-yellow-800';
    case 'CarePlan':
      return 'bg-teal-100 text-teal-800';
    case 'DiagnosticReport':
      return 'bg-pink-100 text-pink-800';
    case 'Device':
      return 'bg-gray-100 text-gray-800';
    case 'ImagingStudy':
      return 'bg-violet-100 text-violet-800';
    case 'MedicationRequest':
      return 'bg-purple-100 text-purple-800';
    case 'MedicationAdministration':
      return 'bg-indigo-100 text-indigo-800';
    case 'Specimen':
      return 'bg-blue-100 text-blue-800';
    case 'Vital Signs':
      return 'bg-red-100 text-red-800';
    case 'Laboratory':
      return 'bg-green-100 text-green-800';
    default:
      return 'bg-gray-100 text-gray-800';
  }
};

const VerticalTimeline: React.FC<VerticalTimelineProps> = ({ events, onEventClick }) => {
  const sortedEvents = events.sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime());

  if (sortedEvents.length === 0) {
    return (
      <div className="flex flex-col items-center justify-center py-12">
        <FileText className="w-16 h-16 text-gray-400 mb-4" />
        <p className="text-gray-500 text-lg">No timeline events found for this patient.</p>
      </div>
    );
  }

  return (
    <div className="max-h-[600px] overflow-auto relative">
      <h2 className="text-2xl font-bold text-healthcare-blue mb-6">Patient Timeline</h2>

      <div className="relative">
        {/* Central timeline line */}
        <div className="absolute left-8 top-0 bottom-0 w-0.5 bg-gradient-to-b from-healthcare-blue to-healthcare-blue-light"></div>

        <div className="space-y-8">
          {sortedEvents.map((event) => {
            const IconComponent = getEventIcon(event.resourceType);
            const gradientClass = getEventGradient(event.resourceType);
            const badgeClass = getBadgeColor(event.resourceType);

            return (
              <div key={event.id} className="relative flex items-start">
                {/* Timeline icon */}
                <div className={`absolute left-0 w-16 h-16 rounded-full bg-gradient-to-br ${gradientClass} flex items-center justify-center shadow-lg border-4 border-white z-10`}>
                  <IconComponent className="w-8 h-8 text-white" />
                </div>

                {/* Content card */}
                <div className="ml-24 flex-1">
                  <div
                    className="bg-white rounded-xl shadow-lg border border-gray-200 p-6 cursor-pointer hover:shadow-xl transition-all duration-300 hover:-translate-y-1"
                    onClick={() => onEventClick?.(event)}
                  >
                    {/* Header with title and badge */}
                    <div className="flex items-start justify-between mb-4">
                      <div className="flex-1">
                        <h3 className="text-xl font-semibold text-gray-900 mb-2 leading-tight">
                          {event.title}
                        </h3>
                        <div className="flex items-center gap-3 text-sm text-gray-600">
                          <div className="flex items-center gap-1">
                            <Calendar className="w-4 h-4" />
                            <span>
                              {new Date(event.date).toLocaleDateString('en-US', {
                                year: 'numeric',
                                month: 'long',
                                day: 'numeric',
                              })}
                            </span>
                          </div>
                          {event.date.includes('T') && (
                            <div className="flex items-center gap-1">
                              <Clock className="w-4 h-4" />
                              <span>
                                {new Date(event.date).toLocaleTimeString('en-US', {
                                  hour: '2-digit',
                                  minute: '2-digit',
                                })}
                              </span>
                            </div>
                          )}
                        </div>
                      </div>
                      <div className={`px-3 py-1 rounded-full text-xs font-medium ${badgeClass} ml-4`}>
                        {event.resourceType}
                      </div>
                    </div>

                    {/* Description */}
                    {event.description && (
                      <div className="mb-4">
                        <p className="text-gray-700 leading-relaxed">
                          {event.description.length > 200
                            ? `${event.description.substring(0, 200)}...`
                            : event.description}
                        </p>
                      </div>
                    )}

                    {/* Clinical details section */}
                    {event.details && (
                      <div className="border-t border-gray-100 pt-4">
                        <div className="flex items-center gap-2 mb-2">
                          <Stethoscope className="w-4 h-4 text-healthcare-blue" />
                          <span className="text-sm font-medium text-healthcare-blue">Clinical Details</span>
                        </div>
                        <div className="bg-gray-50 rounded-lg p-3">
                          <p className="text-sm text-gray-700 whitespace-pre-wrap">
                            {event.details.length > 300
                              ? `${event.details.substring(0, 300)}...`
                              : event.details}
                          </p>
                        </div>
                      </div>
                    )}

                    {/* Resource type indicator */}
                    <div className="mt-4 pt-3 border-t border-gray-100">
                      <div className="flex items-center justify-between text-xs text-gray-500">
                        <span>Resource Type: {event.resourceType}</span>
                        <span>ID: {event.id}</span>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            );
          })}
        </div>
      </div>
    </div>
  );
};

export default VerticalTimeline;