export interface Investor {
    id: number;
    name: string;
    investorType: string;
    dateAdded: string;
    country: string;
    totalCommitmentAmount: number;
    commitments?: Commitment[];
}

export interface Commitment {
    id: number;
    assetClass: string;
    currency: string;
    amount: number;
}


const API_BASE_URL ="https://localhost:7251/api"

async function fetchJson(url: string) {
    const response = await fetch(`${API_BASE_URL}/${url}`);
    if (!response.ok) {
        throw new Error(`Error fetching ${url}`);
    }
    return response.json();
}

export async function fetchInvestors(): Promise<Investor[]> {
    try {
        return await fetchJson('Investors');
    } catch (error) {
        console.error(error);
        return [];
    }
}

export async function fetchInvestorDetails(id: number): Promise<Investor | null> {
    try {
        return await fetchJson(`Investors/${id}`);
    } catch (error) {
        console.error(error);
        return null;
    }
}