import React, { useEffect, useState } from 'react';
import { fetchInvestors, Investor } from '../api';
import { useNavigate } from 'react-router-dom';
import { formatCurrency } from './CurrencyFormatter';


const InvestorsTable: React.FC = () => {
  const [investors, setInvestors] = useState<Investor[]>([]);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    const getInvestors = async () => {
      try {
        const investorsData = await fetchInvestors();
        setInvestors(investorsData);
      } catch (err) {
        setError('Error fetching investors.');
      }
    };

    getInvestors();
  }, []);

  if (error) {
    return <div>{error}</div>;
  }

  if (investors.length === 0) {
    return <div>Loading investors...</div>;
  }

  return (
    <div>
      <h1>Investors</h1>
      <table role="table" aria-label="investors">
        <thead>
          <tr>
            <th>Id</th>
            <th>Name</th>
            <th>Type</th>
            <th>Date Added</th>
            <th>Address</th>
            <th>Total Commitment</th>
          </tr>
        </thead>
        <tbody>
          {investors.map((investor) => (
            <tr key={investor.id} 
                onClick={() => navigate(`/investors/${investor.id}`)}
                style={{ cursor: 'pointer' }}>
              <td>{investor.id}</td>
              <td>{investor.name}</td>
              <td>{investor.investorType}</td>
              <td>{investor.dateAdded}</td>
              <td>{investor.country}</td>
              <td>{formatCurrency(investor.totalCommitmentAmount)}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default InvestorsTable;
