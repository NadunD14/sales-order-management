import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { Provider } from 'react-redux';
import { store } from './redux/store';
import Home from './pages/Home';
import SalesOrderForm from './pages/SalesOrderForm';
import './index.css';

function App() {
    return (
        <Provider store={store}>
            <Router>
                <div className="App">
                    <Routes>
                        <Route path="/" element={<Home />} />
                        <Route path="/sales-order" element={<SalesOrderForm />} />
                        <Route path="/sales-order/:id" element={<SalesOrderForm />} />
                    </Routes>
                </div>
            </Router>
        </Provider>
    );
}

export default App;