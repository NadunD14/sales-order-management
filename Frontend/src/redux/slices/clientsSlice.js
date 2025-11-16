import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { clientsApi } from '../../services/api';

// Async thunks
export const fetchClients = createAsyncThunk(
    'clients/fetchClients',
    async () => {
        const response = await clientsApi.getAll();
        return response.data;
    }
);

export const fetchClientById = createAsyncThunk(
    'clients/fetchClientById',
    async (id) => {
        const response = await clientsApi.getById(id);
        return response.data;
    }
);

const clientsSlice = createSlice({
    name: 'clients',
    initialState: {
        clients: [],
        selectedClient: null,
        loading: false,
        error: null,
    },
    reducers: {
        clearSelectedClient: (state) => {
            state.selectedClient = null;
        },
        clearError: (state) => {
            state.error = null;
        },
    },
    extraReducers: (builder) => {
        builder
            // Fetch all clients
            .addCase(fetchClients.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(fetchClients.fulfilled, (state, action) => {
                state.loading = false;
                state.clients = action.payload;
            })
            .addCase(fetchClients.rejected, (state, action) => {
                state.loading = false;
                state.error = action.error.message;
            })
            // Fetch client by ID
            .addCase(fetchClientById.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(fetchClientById.fulfilled, (state, action) => {
                state.loading = false;
                state.selectedClient = action.payload;
            })
            .addCase(fetchClientById.rejected, (state, action) => {
                state.loading = false;
                state.error = action.error.message;
            });
    },
});

export const { clearSelectedClient, clearError } = clientsSlice.actions;
export default clientsSlice.reducer;