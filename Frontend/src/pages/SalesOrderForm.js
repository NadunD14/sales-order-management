import React, { useState, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate, useParams } from 'react-router-dom';
import { fetchClients } from '../redux/slices/clientsSlice';
import { fetchItems } from '../redux/slices/itemsSlice';
import {
    createSalesOrder,
    updateSalesOrder,
    fetchSalesOrderById,
    generateInvoiceNumber,
    clearSelectedSalesOrder
} from '../redux/slices/salesOrdersSlice'; const SalesOrderForm = () => {
    const dispatch = useDispatch();
    const navigate = useNavigate();
    const { id } = useParams();

    const { clients } = useSelector((state) => state.clients);
    const { items } = useSelector((state) => state.items);
    const { selectedSalesOrder, loading, generatedInvoiceNumber } = useSelector((state) => state.salesOrders);

    const [formData, setFormData] = useState({
        clientId: '',
        invoiceNo: '',
        invoiceDate: new Date().toISOString().split('T')[0],
        referenceNo: '',
        note: '',
        address1: '',
        address2: '',
        address3: '',
        suburb: '',
        state: '',
        postCode: '',
    });

    const [orderItems, setOrderItems] = useState([]);
    const [totals, setTotals] = useState({
        totalExcl: 0,
        totalTax: 0,
        totalIncl: 0,
    });

    useEffect(() => {
        dispatch(fetchClients());
        dispatch(fetchItems());

        if (id) {
            dispatch(fetchSalesOrderById(id));
        } else {
            dispatch(generateInvoiceNumber());
        }

        return () => {
            dispatch(clearSelectedSalesOrder());
        };
    }, [dispatch, id]);

    useEffect(() => {
        if (generatedInvoiceNumber && !id) {
            setFormData(prev => ({ ...prev, invoiceNo: generatedInvoiceNumber }));
        }
    }, [generatedInvoiceNumber, id]);

    useEffect(() => {
        if (selectedSalesOrder && id) {
            setFormData({
                clientId: selectedSalesOrder.clientId,
                invoiceNo: selectedSalesOrder.invoiceNo,
                invoiceDate: selectedSalesOrder.invoiceDate.split('T')[0],
                referenceNo: selectedSalesOrder.referenceNo || '',
                note: selectedSalesOrder.note || '',
                address1: selectedSalesOrder.address1 || '',
                address2: selectedSalesOrder.address2 || '',
                address3: selectedSalesOrder.address3 || '',
                suburb: selectedSalesOrder.suburb || '',
                state: selectedSalesOrder.state || '',
                postCode: selectedSalesOrder.postCode || '',
            });

            const mappedItems = selectedSalesOrder.items.map(item => ({
                id: item.id,
                itemId: item.itemId,
                itemCode: item.itemCode,
                description: item.itemDescription,
                note: item.note || '',
                quantity: item.quantity,
                price: item.price,
                taxRate: item.taxRate,
                exclAmount: item.exclAmount,
                taxAmount: item.taxAmount,
                inclAmount: item.inclAmount,
            }));
            setOrderItems(mappedItems);
        }
    }, [selectedSalesOrder, id]);

    useEffect(() => {
        const totalExcl = orderItems.reduce((sum, item) => sum + item.exclAmount, 0);
        const totalTax = orderItems.reduce((sum, item) => sum + item.taxAmount, 0);
        const totalIncl = totalExcl + totalTax;

        setTotals({ totalExcl, totalTax, totalIncl });
    }, [orderItems]);

    const handleClientChange = (e) => {
        const clientId = e.target.value;
        setFormData(prev => ({ ...prev, clientId }));

        if (clientId) {
            const selectedClient = clients.find(c => c.id === parseInt(clientId));
            if (selectedClient) {
                setFormData(prev => ({
                    ...prev,
                    address1: selectedClient.address1 || '',
                    address2: selectedClient.address2 || '',
                    address3: selectedClient.address3 || '',
                    suburb: selectedClient.suburb || '',
                    state: selectedClient.state || '',
                    postCode: selectedClient.postCode || '',
                }));
            }
        }
    };

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const addOrderItem = () => {
        const newItem = {
            id: Date.now(),
            itemId: '',
            itemCode: '',
            description: '',
            note: '',
            quantity: 1,
            price: 0,
            taxRate: 10,
            exclAmount: 0,
            taxAmount: 0,
            inclAmount: 0,
        };
        setOrderItems(prev => [...prev, newItem]);
    };

    const removeOrderItem = (index) => {
        setOrderItems(prev => prev.filter((_, i) => i !== index));
    };

    const handleItemChange = (index, field, value) => {
        setOrderItems(prev => {
            const newItems = [...prev];
            newItems[index] = { ...newItems[index], [field]: value };

            if (field === 'itemId') {
                const selectedItem = items.find(item => item.id === parseInt(value));
                if (selectedItem) {
                    newItems[index].itemCode = selectedItem.itemCode;
                    newItems[index].description = selectedItem.description;
                    newItems[index].price = selectedItem.price;
                }
            }

            if (field === 'itemCode') {
                const selectedItem = items.find(item => item.itemCode === value);
                if (selectedItem) {
                    newItems[index].itemId = selectedItem.id;
                    newItems[index].description = selectedItem.description;
                    newItems[index].price = selectedItem.price;
                }
            }

            // Recalculate amounts
            const item = newItems[index];
            item.exclAmount = item.quantity * item.price;
            item.taxAmount = item.exclAmount * item.taxRate / 100;
            item.inclAmount = item.exclAmount + item.taxAmount;

            return newItems;
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const salesOrderData = {
            ...formData,
            clientId: parseInt(formData.clientId),
            invoiceDate: new Date(formData.invoiceDate).toISOString(),
            items: orderItems.map(item => ({
                itemId: item.itemId,
                note: item.note,
                quantity: item.quantity,
                taxRate: item.taxRate,
            })),
        };

        try {
            if (id) {
                await dispatch(updateSalesOrder({ id: parseInt(id), data: { ...salesOrderData, id: parseInt(id) } }));
            } else {
                await dispatch(createSalesOrder(salesOrderData));
            }
            navigate('/');
        } catch (error) {
            console.error('Error saving sales order:', error);
        }
    };

    const handleCancel = () => {
        navigate('/');
    };

    if (loading) {
        return <div className="flex justify-center items-center h-64">Loading...</div>;
    }

    return (
        <div className="min-h-screen bg-gray-50 p-6">
            <div className="max-w-7xl mx-auto bg-white rounded-lg shadow-lg">
                {/* Header */}
                <div className="bg-blue-600 text-white p-4 rounded-t-lg">
                    <h1 className="text-xl font-semibold">Sales Order</h1>
                </div>

                {/* Form */}
                <form onSubmit={handleSubmit} className="p-6">
                    {/* Action Buttons */}
                    <div className="mb-6 flex gap-4">
                        <button
                            type="submit"
                            className="bg-green-600 hover:bg-green-700 text-white px-4 py-2 rounded flex items-center gap-2"
                            disabled={loading}
                        >
                            <span>✓</span>
                            Save Order
                        </button>
                        <button
                            type="button"
                            onClick={handleCancel}
                            className="bg-gray-600 hover:bg-gray-700 text-white px-4 py-2 rounded"
                        >
                            Cancel
                        </button>
                    </div>

                    {/* Customer and Invoice Details */}
                    <div className="grid grid-cols-1 lg:grid-cols-2 gap-6 mb-6">
                        {/* Left Column - Customer Details */}
                        <div className="space-y-4">
                            <div>
                                <label className="block text-sm font-medium text-gray-700 mb-1">
                                    Customer Name
                                </label>
                                <select
                                    name="clientId"
                                    value={formData.clientId}
                                    onChange={handleClientChange}
                                    className="w-full p-2 border border-gray-300 rounded focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                                    required
                                >
                                    <option value="">Select Customer</option>
                                    {clients.map(client => (
                                        <option key={client.id} value={client.id}>
                                            {client.customerName}
                                        </option>
                                    ))}
                                </select>
                            </div>

                            <div>
                                <label className="block text-sm font-medium text-gray-700 mb-1">
                                    Address 1
                                </label>
                                <input
                                    type="text"
                                    name="address1"
                                    value={formData.address1}
                                    onChange={handleInputChange}
                                    className="w-full p-2 border border-gray-300 rounded focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                                />
                            </div>

                            <div>
                                <label className="block text-sm font-medium text-gray-700 mb-1">
                                    Address 2
                                </label>
                                <input
                                    type="text"
                                    name="address2"
                                    value={formData.address2}
                                    onChange={handleInputChange}
                                    className="w-full p-2 border border-gray-300 rounded focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                                />
                            </div>

                            <div>
                                <label className="block text-sm font-medium text-gray-700 mb-1">
                                    Address 3
                                </label>
                                <input
                                    type="text"
                                    name="address3"
                                    value={formData.address3}
                                    onChange={handleInputChange}
                                    className="w-full p-2 border border-gray-300 rounded focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                                />
                            </div>

                            <div>
                                <label className="block text-sm font-medium text-gray-700 mb-1">
                                    Suburb
                                </label>
                                <input
                                    type="text"
                                    name="suburb"
                                    value={formData.suburb}
                                    onChange={handleInputChange}
                                    className="w-full p-2 border border-gray-300 rounded focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                                />
                            </div>

                            <div>
                                <label className="block text-sm font-medium text-gray-700 mb-1">
                                    State
                                </label>
                                <input
                                    type="text"
                                    name="state"
                                    value={formData.state}
                                    onChange={handleInputChange}
                                    className="w-full p-2 border border-gray-300 rounded focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                                />
                            </div>

                            <div>
                                <label className="block text-sm font-medium text-gray-700 mb-1">
                                    Post Code
                                </label>
                                <input
                                    type="text"
                                    name="postCode"
                                    value={formData.postCode}
                                    onChange={handleInputChange}
                                    className="w-full p-2 border border-gray-300 rounded focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                                />
                            </div>
                        </div>

                        {/* Right Column - Invoice Details */}
                        <div className="space-y-4">
                            <div>
                                <label className="block text-sm font-medium text-gray-700 mb-1">
                                    Invoice No.
                                </label>
                                <input
                                    type="text"
                                    name="invoiceNo"
                                    value={formData.invoiceNo}
                                    onChange={handleInputChange}
                                    className="w-full p-2 border border-gray-300 rounded focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                                    required
                                />
                            </div>

                            <div>
                                <label className="block text-sm font-medium text-gray-700 mb-1">
                                    Invoice Date
                                </label>
                                <input
                                    type="date"
                                    name="invoiceDate"
                                    value={formData.invoiceDate}
                                    onChange={handleInputChange}
                                    className="w-full p-2 border border-gray-300 rounded focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                                    required
                                />
                            </div>

                            <div>
                                <label className="block text-sm font-medium text-gray-700 mb-1">
                                    Reference no
                                </label>
                                <input
                                    type="text"
                                    name="referenceNo"
                                    value={formData.referenceNo}
                                    onChange={handleInputChange}
                                    className="w-full p-2 border border-gray-300 rounded focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                                />
                            </div>

                            <div>
                                <label className="block text-sm font-medium text-gray-700 mb-1">
                                    Note
                                </label>
                                <textarea
                                    name="note"
                                    value={formData.note}
                                    onChange={handleInputChange}
                                    rows="4"
                                    className="w-full p-2 border border-gray-300 rounded focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                                />
                            </div>
                        </div>
                    </div>

                    {/* Items Table */}
                    <div className="mb-6">
                        <div className="flex justify-between items-center mb-4">
                            <h3 className="text-lg font-semibold">Order Items</h3>
                            <button
                                type="button"
                                onClick={addOrderItem}
                                className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded"
                            >
                                Add Item
                            </button>
                        </div>

                        <div className="overflow-x-auto">
                            <table className="w-full border border-gray-300">
                                <thead className="bg-gray-100">
                                    <tr>
                                        <th className="border border-gray-300 p-2 text-left">Item Code</th>
                                        <th className="border border-gray-300 p-2 text-left">Description</th>
                                        <th className="border border-gray-300 p-2 text-left">Note</th>
                                        <th className="border border-gray-300 p-2 text-left">Quantity</th>
                                        <th className="border border-gray-300 p-2 text-left">Price</th>
                                        <th className="border border-gray-300 p-2 text-left">Tax</th>
                                        <th className="border border-gray-300 p-2 text-left">Excl Amount</th>
                                        <th className="border border-gray-300 p-2 text-left">Tax Amount</th>
                                        <th className="border border-gray-300 p-2 text-left">Incl Amount</th>
                                        <th className="border border-gray-300 p-2 text-left">Actions</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {orderItems.map((item, index) => (
                                        <tr key={item.id} className="bg-white">
                                            <td className="border border-gray-300 p-2">
                                                <select
                                                    value={item.itemCode}
                                                    onChange={(e) => handleItemChange(index, 'itemCode', e.target.value)}
                                                    className="w-full p-1 border rounded text-sm"
                                                >
                                                    <option value="">Select Item</option>
                                                    {items.map(itm => (
                                                        <option key={itm.id} value={itm.itemCode}>
                                                            {itm.itemCode}
                                                        </option>
                                                    ))}
                                                </select>
                                            </td>
                                            <td className="border border-gray-300 p-2">
                                                <select
                                                    value={item.description}
                                                    onChange={(e) => handleItemChange(index, 'itemId', items.find(i => i.description === e.target.value)?.id || '')}
                                                    className="w-full p-1 border rounded text-sm"
                                                >
                                                    <option value="">Select Item</option>
                                                    {items.map(itm => (
                                                        <option key={itm.id} value={itm.description}>
                                                            {itm.description}
                                                        </option>
                                                    ))}
                                                </select>
                                            </td>
                                            <td className="border border-gray-300 p-2">
                                                <input
                                                    type="text"
                                                    value={item.note}
                                                    onChange={(e) => handleItemChange(index, 'note', e.target.value)}
                                                    className="w-full p-1 border rounded text-sm"
                                                />
                                            </td>
                                            <td className="border border-gray-300 p-2">
                                                <input
                                                    type="number"
                                                    value={item.quantity}
                                                    onChange={(e) => handleItemChange(index, 'quantity', parseFloat(e.target.value) || 0)}
                                                    className="w-full p-1 border rounded text-sm"
                                                    min="0"
                                                    step="0.01"
                                                />
                                            </td>
                                            <td className="border border-gray-300 p-2">
                                                <input
                                                    type="number"
                                                    value={item.price}
                                                    readOnly
                                                    className="w-full p-1 border rounded text-sm bg-gray-100"
                                                    step="0.01"
                                                />
                                            </td>
                                            <td className="border border-gray-300 p-2">
                                                <input
                                                    type="number"
                                                    value={item.taxRate}
                                                    onChange={(e) => handleItemChange(index, 'taxRate', parseFloat(e.target.value) || 0)}
                                                    className="w-full p-1 border rounded text-sm"
                                                    min="0"
                                                    step="0.01"
                                                />
                                            </td>
                                            <td className="border border-gray-300 p-2">
                                                <input
                                                    type="number"
                                                    value={item.exclAmount.toFixed(2)}
                                                    readOnly
                                                    className="w-full p-1 border rounded text-sm bg-gray-100"
                                                />
                                            </td>
                                            <td className="border border-gray-300 p-2">
                                                <input
                                                    type="number"
                                                    value={item.taxAmount.toFixed(2)}
                                                    readOnly
                                                    className="w-full p-1 border rounded text-sm bg-gray-100"
                                                />
                                            </td>
                                            <td className="border border-gray-300 p-2">
                                                <input
                                                    type="number"
                                                    value={item.inclAmount.toFixed(2)}
                                                    readOnly
                                                    className="w-full p-1 border rounded text-sm bg-gray-100"
                                                />
                                            </td>
                                            <td className="border border-gray-300 p-2">
                                                <button
                                                    type="button"
                                                    onClick={() => removeOrderItem(index)}
                                                    className="bg-red-500 hover:bg-red-600 text-white px-2 py-1 rounded text-xs"
                                                >
                                                    Remove
                                                </button>
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    </div>

                    {/* Totals */}
                    <div className="flex justify-end">
                        <div className="w-80">
                            <div className="space-y-2">
                                <div className="flex justify-between">
                                    <span className="font-medium">Total Excl</span>
                                    <input
                                        type="text"
                                        value={totals.totalExcl.toFixed(2)}
                                        readOnly
                                        className="w-32 p-2 border border-gray-300 rounded bg-gray-100 text-right"
                                    />
                                </div>
                                <div className="flex justify-between">
                                    <span className="font-medium">Total Tax</span>
                                    <input
                                        type="text"
                                        value={totals.totalTax.toFixed(2)}
                                        readOnly
                                        className="w-32 p-2 border border-gray-300 rounded bg-gray-100 text-right"
                                    />
                                </div>
                                <div className="flex justify-between">
                                    <span className="font-medium">Total Incl</span>
                                    <input
                                        type="text"
                                        value={totals.totalIncl.toFixed(2)}
                                        readOnly
                                        className="w-32 p-2 border border-gray-300 rounded bg-gray-100 text-right font-bold"
                                    />
                                </div>
                            </div>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default SalesOrderForm;