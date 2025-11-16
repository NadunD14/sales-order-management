import axios from 'axios';

const API_BASE_URL = process.env.REACT_APP_API_URL || 'http://localhost:5000/api';

const apiClient = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        'Content-Type': 'application/json',
    },
});

export const clientsApi = {
    getAll: () => apiClient.get('/clients'),
    getById: (id) => apiClient.get(`/clients/${id}`),
};

export const itemsApi = {
    getAll: () => apiClient.get('/items'),
    getById: (id) => apiClient.get(`/items/${id}`),
};

export const salesOrdersApi = {
    getAll: () => apiClient.get('/salesorders'),
    getById: (id) => apiClient.get(`/salesorders/${id}`),
    create: (data) => apiClient.post('/salesorders', data),
    update: (id, data) => apiClient.put(`/salesorders/${id}`, data),
    delete: (id) => apiClient.delete(`/salesorders/${id}`),
    generateInvoiceNumber: () => apiClient.get('/salesorders/generate-invoice-number'),
};

export default apiClient;