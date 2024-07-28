import React from 'react';
import { useParams } from 'react-router-dom';
import InvestorDetails from '../components/InvestorDetails';

const InvestorPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();

    return (
        <div>
            <h1>Investor Details</h1>
            <InvestorDetails id={parseInt(id!, 10)} />
        </div>
    );
};

export default InvestorPage;
