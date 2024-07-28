import React from 'react';
import { render, screen, fireEvent, within } from '@testing-library/react';
import { fetchInvestors, Investor } from '../api';
import { MemoryRouter, Route, Routes } from 'react-router-dom';
import InvestorsTable from '../components/InvestorsTable';
import InvestorDetails from '../components/InvestorDetails';

jest.mock('../api');

const mockFetchInvestors = fetchInvestors as jest.MockedFunction<typeof fetchInvestors>;

const mockInvestors: Investor[] = [
  {
    id: 1,
    name: 'Investor One',
    investorType: 'Private Equity',
    dateAdded: '2023-01-01',
    country: 'Address 1',
    totalCommitmentAmount: 1500000,
  },
  {
    id: 2,
    name: 'Investor Two',
    investorType: 'Private Debt',
    dateAdded: '2023-02-01',
    country: 'Address 2',
    totalCommitmentAmount: 2000000,
  },
];

describe('Investors', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  test('renders investors table', async () => {
    mockFetchInvestors.mockResolvedValue(mockInvestors);

    render(
      <MemoryRouter>
        <InvestorsTable />
      </MemoryRouter>
    );

    expect(screen.getByText(/Loading investors.../i)).toBeInTheDocument();

    expect(await screen.findByText('Investors')).toBeInTheDocument();
    const investorsTable = screen.getByRole('table', { name: 'investors' });
    expect(within(investorsTable).getByText('Investor One')).toBeInTheDocument();
    expect(within(investorsTable).getByText('Investor Two')).toBeInTheDocument();
    expect(within(investorsTable).getByText('Private Equity')).toBeInTheDocument();
    expect(within(investorsTable).getByText('Private Debt')).toBeInTheDocument();
    expect(within(investorsTable).getByText('2023-01-01')).toBeInTheDocument();
    expect(within(investorsTable).getByText('2023-02-01')).toBeInTheDocument();
    expect(within(investorsTable).getByText('Address 1')).toBeInTheDocument();
    expect(within(investorsTable).getByText('Address 2')).toBeInTheDocument();
    expect(within(investorsTable).getByText('£1,500,000.00')).toBeInTheDocument();
    expect(within(investorsTable).getByText('£2,000,000.00')).toBeInTheDocument();
  });

  test('handles error when fetching investors', async () => {
    mockFetchInvestors.mockRejectedValue(new Error('Error fetching investors'));

    render(
      <MemoryRouter>
        <InvestorsTable />
      </MemoryRouter>
    );

    expect(await screen.findByText('Error fetching investors.')).toBeInTheDocument();
  });

  test('navigates to investor details on row click', async () => {
    mockFetchInvestors.mockResolvedValue(mockInvestors);

    render(
      <MemoryRouter initialEntries={['/']}>
        <Routes>
          <Route path="/" element={<InvestorsTable />} />
          <Route path="/investors/:id" element={<InvestorDetails id={1} />} />
        </Routes>
      </MemoryRouter>
    );

    const investorsTable = await screen.findByRole('table', { name: 'investors' });
    const firstInvestorRow = within(investorsTable).getByText('Investor One');
    fireEvent.click(firstInvestorRow);

    expect(await screen.findByText('Loading investor details...')).toBeInTheDocument();
  });
});
