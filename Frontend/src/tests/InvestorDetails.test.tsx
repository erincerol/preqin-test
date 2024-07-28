import React from 'react';
import { render, screen, fireEvent, within } from '@testing-library/react';
import InvestorDetails from '../components/InvestorDetails';
import { fetchInvestorDetails, Investor, Commitment } from '../api';

jest.mock('../api');

const mockFetchInvestorDetails = fetchInvestorDetails as jest.MockedFunction<typeof fetchInvestorDetails>;

const mockInvestor: Investor = {
    id: 1,
    name: 'Investor One',
    investorType: 'Private Equity',
    dateAdded: '2023-01-01',
    country: 'Address 1',
    totalCommitmentAmount: 1500000,
    commitments:[
        {
            id: 1,
            assetClass: 'Private Equity',
            currency: 'GBP',
            amount: 1000000,
        },
        {
            id: 2,
            assetClass: 'Private Debt',
            currency: 'GBP',
            amount: 500000,
        },
    ]    
};

describe('InvestorDetails', () => {
    beforeEach(() => {
        jest.clearAllMocks();
    });

    test('renders investor details', async () => {
        mockFetchInvestorDetails.mockResolvedValue(mockInvestor);

        render(<InvestorDetails id={1} />);

        expect(screen.getByText(/Loading investor details.../i)).toBeInTheDocument();

        expect(await screen.findByText('Investor One')).toBeInTheDocument();
        expect(screen.getByText('Type: Private Equity')).toBeInTheDocument();
        expect(screen.getByText('Date Added: 2023-01-01')).toBeInTheDocument();
        expect(screen.getByText('Address: Address 1')).toBeInTheDocument();
        expect(screen.getByText('Total Commitment: £1,500,000.00')).toBeInTheDocument();
    });

    test('renders commitments and calculates total amount', async () => {
        mockFetchInvestorDetails.mockResolvedValue(mockInvestor);

        render(<InvestorDetails id={1} />);

        expect(await screen.findByText('Total Amount for All Asset Class: £1,500,000.00')).toBeInTheDocument();
        expect(screen.getByText('Commitment Information')).toBeInTheDocument();

        const commitmentsTable = screen.getByRole('table', { name: 'commitments' });
        expect(within(commitmentsTable).getByText('Private Equity')).toBeInTheDocument();
        expect(within(commitmentsTable).getByText('Private Debt')).toBeInTheDocument();
        expect(within(commitmentsTable).getByText('£1,000,000.00')).toBeInTheDocument();
        expect(within(commitmentsTable).getByText('£500,000.00')).toBeInTheDocument();
    });

    test('updates commitments when an asset class is selected', async () => {
        mockFetchInvestorDetails.mockResolvedValue(mockInvestor);

        render(<InvestorDetails id={1} />);

        expect(await screen.findByText('Total Amount for All Asset Class: £1,500,000.00')).toBeInTheDocument();

        const privateEquityButton = screen.getByRole('button', { name: /Private Equity/i });
        fireEvent.click(privateEquityButton);

        expect(await screen.findByText('Total Amount for Private Equity Asset Class: £1,000,000.00')).toBeInTheDocument();

        const commitmentsTable = screen.getByRole('table', { name: 'commitments' });
        expect(within(commitmentsTable).getByText('Private Equity')).toBeInTheDocument();
        expect(within(commitmentsTable).queryByText('Private Debt')).not.toBeInTheDocument();
        expect(within(commitmentsTable).getByText('£1,000,000.00')).toBeInTheDocument();
        expect(within(commitmentsTable).queryByText('£500,000.00')).not.toBeInTheDocument();
    });

    test('handles error when fetching investor details', async () => {
        mockFetchInvestorDetails.mockRejectedValue(new Error('Error fetching investor details'));

        render(<InvestorDetails id={1} />);

        expect(await screen.findByText('Error fetching investor details.')).toBeInTheDocument();
    });
});
