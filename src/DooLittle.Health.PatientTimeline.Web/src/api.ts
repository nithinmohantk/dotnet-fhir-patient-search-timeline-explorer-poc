import axios from 'axios';
import type { Patient, FHIRTimelineData } from './types';

const API_BASE_URL = 'http://localhost:8080/api';

const api = axios.create({
  baseURL: API_BASE_URL,
});

export const getPatients = async (): Promise<Patient[]> => {
  const response = await api.get('/patients');
  return response.data;
};

export const getPatient = async (id: number): Promise<Patient> => {
  const response = await api.get(`/patients/${id}`);
  return response.data;
};

export const getPatientTimeline = async (syntheaId: string): Promise<FHIRTimelineData> => {
  const response = await api.get(`/patients/by-synthea/${syntheaId}/timeline`);
  return response.data;
};

export const createPatient = async (patient: Omit<Patient, 'id'>): Promise<Patient> => {
  const response = await api.post('/patients', patient);
  return response.data;
};