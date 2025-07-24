import { useEffect, useState } from "react";
import styled from "styled-components";
import loyaltyAPI from "@/services/loyaltyAPI";
import useAuth from "@/hooks/useAuth";
import AccountCus from "@/components/Customer/Account Details/AccountCus";

const Loyalty = () => {
  const { AccountID } = useAuth();
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  const [loyaltyPoints, setLoyaltyPoints] = useState<any>(null);

  useEffect(() => {
    if (AccountID && AccountID !== 0) {
      const fetchLoyaltyPoints = async () => {
        try {
          const { data } = await loyaltyAPI.getLoyaltyPointByUserId(
            AccountID.toString()
          );
          setLoyaltyPoints(data);
        } catch (error) {
          console.error("Error fetching loyalty points:", error);
        }
      };
      fetchLoyaltyPoints();
    }
  }, [AccountID]);

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return new Intl.DateTimeFormat("en-GB", {
      year: "numeric",
      month: "2-digit",
      day: "2-digit",
    }).format(date);
  };

  return (
    <div>
      <AccountCus />
      <MainContainer>
        <Section>
          <ProfileTitle>Loyalty Points</ProfileTitle>
          <InfoSection>
            <InfoContainer>
              <Column>
                <Row>
                  <InfoTitle>Your Loyalty Details</InfoTitle>
                </Row>
                {loyaltyPoints ? (
                  <DataGrid>
                    <DataColumn>
                      <DetailGroup>
                        <Label>POINTS EARNED</Label>
                        <Detail>{loyaltyPoints.pointsEarned}</Detail>
                      </DetailGroup>
                      <DetailGroup>
                        <Label>POINTS REDEEMED</Label>
                        <Detail>{loyaltyPoints.pointsRedeemed}</Detail>
                      </DetailGroup>
                      <DetailGroup>
                        <Label>LAST UPDATE</Label>
                        <Detail>
                          {loyaltyPoints.lastUpdated
                            ? formatDate(loyaltyPoints.lastUpdated)
                            : "N/A"}
                        </Detail>
                      </DetailGroup>
                    </DataColumn>
                  </DataGrid>
                ) : (
                  <p>Loading loyalty points...</p>
                )}
              </Column>
            </InfoContainer>
          </InfoSection>
        </Section>
      </MainContainer>
    </div>
  );
};

const MainContainer = styled.div`
  background-color: #fff;
  display: flex;
  flex-direction: column;
  align-items: center;
  min-height: 100vh;
  padding: 20px 0;
`;

const Section = styled.section`
  background-color: #fff;
  color: #000;
  font: 400 15px / 150% "Crimson Text", sans-serif;
  width: 100%;
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 20px;
`;

const ProfileTitle = styled.div`
  position: relative;
  margin: 15px 0 50px;
  font: 600 32px "Crimson Text", sans-serif;
  text-align: center;
  color: #151542;
`;

const InfoSection = styled.section`
  width: 100%;
  max-width: 1000px;
  margin: 0 auto;
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
  padding: 50px;
`;

const InfoContainer = styled.div`
  display: flex;
  flex-direction: column;
  width: 100%;
`;

const Column = styled.div`
  display: flex;
  flex-direction: column;
  width: 100%;
`;

const Row = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 30px;
  margin-bottom: 40px;
  flex-wrap: wrap;
`;

const InfoTitle = styled.div`
  font-family: "Crimson Text", sans-serif;
  font-size: 28px;
  font-weight: 600;
  color: #151542;
`;

const DetailGroup = styled.div`
  display: flex;
  flex-direction: column;
  margin-bottom: 30px;
  padding: 25px;
  background: #f8f9fa;
  border-radius: 8px;
  border-left: 4px solid #151542;
`;

const Detail = styled.span`
  font-family: "Poppins", sans-serif;
  font-weight: 500;
  letter-spacing: 0.5px;
  margin-top: 10px;
  color: #151542;
  line-height: 1.6;
  font-size: 16px;
`;

const Label = styled.label`
  font-family: "Poppins", sans-serif;
  font-size: 14px;
  font-weight: 600;
  color: #6c757d;
  text-transform: uppercase;
  letter-spacing: 1px;
`;

const DataGrid = styled.div`
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 35px;
  width: 100%;

  @media (max-width: 768px) {
    grid-template-columns: 1fr;
    gap: 20px;
  }
`;

const DataColumn = styled.div`
  display: flex;
  flex-direction: column;
  gap: 3px;
`;

export default Loyalty;
