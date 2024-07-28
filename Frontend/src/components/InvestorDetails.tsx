import React, { useEffect, useState } from 'react';
import { fetchInvestorDetails, Investor, Commitment } from '../api';
import './InvestorDetails.css';
import { formatCurrency } from './CurrencyFormatter';

interface InvestorDetailsProps {
    id: number;
}

const InvestorDetails: React.FC<InvestorDetailsProps> = ({ id }) => {
    const [investorDetails, setInvestorDetails] = useState<Investor | null>(null);
    const [commitments, setCommitments] = useState<Commitment[] | undefined>([]);
    const [totalAmount, setTotalAmount] = useState<number | undefined>(0);
    const [selectedAssetClass, setSelectedAssetClass] = useState('All');
    const [assetClasses, setAssetClasses] = useState(['All']);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        async function getData() {
            try {
                const data = await fetchInvestorDetails(id);
                if (data) {
                    setInvestorDetails(data);
                } else {
                    setError("Investor details not found.");
                }
                const uniqueAssetClasses = Array.from(new Set(data?.commitments?.map(commitment => commitment.assetClass)));
                setAssetClasses(['All', ...uniqueAssetClasses]);
            } catch (error) {
                setError("Error fetching investor details.");
            }
        }
        getData();
    }, [id]);

    useEffect(() => {
        async function getCommitments() {
            try {
                let data: Commitment[] | undefined;
                if (selectedAssetClass === 'All') {
                    data = investorDetails?.commitments
                    setTotalAmount(investorDetails?.totalCommitmentAmount);

                } else {
                    data = investorDetails?.commitments?.filter(commitment => commitment.assetClass === selectedAssetClass);                
                    const total = data?.reduce((sum, commitment) => sum + commitment.amount, 0);
                    setTotalAmount(total);
                }
                setCommitments(data);
            } catch (error) {
                setCommitments([]);
                setError("Error fetching commitments.");
            }
        }
        getCommitments();
    }, [selectedAssetClass, id, investorDetails?.commitments, investorDetails?.totalCommitmentAmount]);

    return (
        <div>
            {error && <p className="error">{error}</p>}
            {investorDetails ? (
                <div>
                    <h2>{investorDetails.name}</h2>
                    <p>Type: {investorDetails.investorType}</p>
                    <p>Date Added: {investorDetails.dateAdded}</p>
                    <p>Address: {investorDetails.country}</p>
                    <p>Total Commitment: {formatCurrency(investorDetails.totalCommitmentAmount)}</p>
                </div>
            ) : (
                <p>Loading investor details...</p>
            )}
            <div className="asset-class-buttons">
                {assetClasses.map(ac => (
                    <button
                        key={ac}
                        className={`asset-class-button ${selectedAssetClass === ac ? 'selected' : ''}`}
                        onClick={() => setSelectedAssetClass(ac)}
                    >
                        {ac}
                    </button>
                ))}
            </div>
            <div>
                <h3>Total Amount for {selectedAssetClass} Asset Class: {totalAmount && formatCurrency(totalAmount)}</h3>
            </div>
            <div>
                <h3>Commitment Information</h3>
                {commitments && commitments.length > 0 ? (
                    <table role="table" aria-label="commitments">
                        <thead>
                            <tr>
                                <th>Id</th>
                                <th>Asset Class</th>
                                <th>Currency</th>
                                <th>Amount</th>
                            </tr>
                        </thead>
                        <tbody>
                            {commitments.map(commitment => (
                                <tr key={commitment.id}>
                                    <td>{commitment.id}</td>
                                    <td>{commitment.assetClass}</td>
                                    <td>{commitment.currency}</td>
                                    <td>{formatCurrency(commitment.amount)}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                ) : (
                    <p>No commitments found.</p>
                )}
            </div>
        </div>
    );
};

export default InvestorDetails;
