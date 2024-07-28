import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import HomePage from './pages/HomePage';
import InvestorPage from './pages/InvestorPage';

const App: React.FC = () => {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<HomePage />} />
                <Route path="/investors/:id" element={<InvestorPage />} />
            </Routes>
        </Router>
    );
};

export default App;
