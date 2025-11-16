import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { salesOrdersApi } from '../../services/api';

// Async thunks
export const fetchSalesOrders = createAsyncThunk(
    'salesOrders/fetchSalesOrders',
    async () => {
        const response = await salesOrdersApi.getAll();
        return response.data;
    }
);

export const fetchSalesOrderById = createAsyncThunk(
    'salesOrders/fetchSalesOrderById',
    async (id) => {
        const response = await salesOrdersApi.getById(id);
        return response.data;
    }
);

export const createSalesOrder = createAsyncThunk(
    'salesOrders/createSalesOrder',
    async (salesOrderData) => {
        const response = await salesOrdersApi.create(salesOrderData);
        return response.data;
    }
);

export const updateSalesOrder = createAsyncThunk(
    'salesOrders/updateSalesOrder',
    async ({ id, data }) => {
        const response = await salesOrdersApi.update(id, data);
        return response.data;
    }
);

export const deleteSalesOrder = createAsyncThunk(
    'salesOrders/deleteSalesOrder',
    async (id) => {
        await salesOrdersApi.delete(id);
        return id;
    }
);

export const generateInvoiceNumber = createAsyncThunk(
    'salesOrders/generateInvoiceNumber',
    async () => {
        const response = await salesOrdersApi.generateInvoiceNumber();
        return response.data;
    }
);

const salesOrdersSlice = createSlice({
    name: 'salesOrders',
    initialState: {
        salesOrders: [],
        selectedSalesOrder: null,
        loading: false,
        error: null,
        generatedInvoiceNumber: null,
    },
    reducers: {
        clearSelectedSalesOrder: (state) => {
            state.selectedSalesOrder = null;
        },
        clearError: (state) => {
            state.error = null;
        },
        clearGeneratedInvoiceNumber: (state) => {
            state.generatedInvoiceNumber = null;
        },
    },
    extraReducers: (builder) => {
        builder
            // Fetch all sales orders
            .addCase(fetchSalesOrders.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(fetchSalesOrders.fulfilled, (state, action) => {
                state.loading = false;
                state.salesOrders = action.payload;
            })
            .addCase(fetchSalesOrders.rejected, (state, action) => {
                state.loading = false;
                state.error = action.error.message;
            })
            // Fetch sales order by ID
            .addCase(fetchSalesOrderById.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(fetchSalesOrderById.fulfilled, (state, action) => {
                state.loading = false;
                state.selectedSalesOrder = action.payload;
            })
            .addCase(fetchSalesOrderById.rejected, (state, action) => {
                state.loading = false;
                state.error = action.error.message;
            })
            // Create sales order
            .addCase(createSalesOrder.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(createSalesOrder.fulfilled, (state, action) => {
                state.loading = false;
                state.salesOrders.unshift(action.payload);
            })
            .addCase(createSalesOrder.rejected, (state, action) => {
                state.loading = false;
                state.error = action.error.message;
            })
            // Update sales order
            .addCase(updateSalesOrder.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(updateSalesOrder.fulfilled, (state, action) => {
                state.loading = false;
                const index = state.salesOrders.findIndex(so => so.id === action.payload.id);
                if (index !== -1) {
                    state.salesOrders[index] = action.payload;
                }
                state.selectedSalesOrder = action.payload;
            })
            .addCase(updateSalesOrder.rejected, (state, action) => {
                state.loading = false;
                state.error = action.error.message;
            })
            // Delete sales order
            .addCase(deleteSalesOrder.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(deleteSalesOrder.fulfilled, (state, action) => {
                state.loading = false;
                state.salesOrders = state.salesOrders.filter(so => so.id !== action.payload);
            })
            .addCase(deleteSalesOrder.rejected, (state, action) => {
                state.loading = false;
                state.error = action.error.message;
            })
            // Generate invoice number
            .addCase(generateInvoiceNumber.fulfilled, (state, action) => {
                state.generatedInvoiceNumber = action.payload;
            });
    },
});

export const { clearSelectedSalesOrder, clearError, clearGeneratedInvoiceNumber } = salesOrdersSlice.actions;
export default salesOrdersSlice.reducer;