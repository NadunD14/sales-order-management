import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { itemsApi } from '../../services/api';

// Async thunks
export const fetchItems = createAsyncThunk(
    'items/fetchItems',
    async () => {
        const response = await itemsApi.getAll();
        return response.data;
    }
);

export const fetchItemById = createAsyncThunk(
    'items/fetchItemById',
    async (id) => {
        const response = await itemsApi.getById(id);
        return response.data;
    }
);

const itemsSlice = createSlice({
    name: 'items',
    initialState: {
        items: [],
        selectedItem: null,
        loading: false,
        error: null,
    },
    reducers: {
        clearSelectedItem: (state) => {
            state.selectedItem = null;
        },
        clearError: (state) => {
            state.error = null;
        },
    },
    extraReducers: (builder) => {
        builder
            // Fetch all items
            .addCase(fetchItems.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(fetchItems.fulfilled, (state, action) => {
                state.loading = false;
                state.items = action.payload;
            })
            .addCase(fetchItems.rejected, (state, action) => {
                state.loading = false;
                state.error = action.error.message;
            })
            // Fetch item by ID
            .addCase(fetchItemById.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(fetchItemById.fulfilled, (state, action) => {
                state.loading = false;
                state.selectedItem = action.payload;
            })
            .addCase(fetchItemById.rejected, (state, action) => {
                state.loading = false;
                state.error = action.error.message;
            });
    },
});

export const { clearSelectedItem, clearError } = itemsSlice.actions;
export default itemsSlice.reducer;