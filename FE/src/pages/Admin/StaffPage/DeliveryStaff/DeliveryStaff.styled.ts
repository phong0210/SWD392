import styled, { createGlobalStyle } from "styled-components";
import { theme } from "../../../../themes";

export const GlobalStyle = createGlobalStyle`
  html, body {
    height: 100%;
    margin: 0;
    padding: 0;
    background-color: #f1f1f1;
    font-family: 'Poppins', sans-serif;
  }
`;

export const AdminArea = styled.section`
  display: inline-flex;
  background-color: #f1f1f1;
  font-family: "Poppins", sans-serif;
  width: 100%;
`;

export const AdminPage = styled.div`
  margin-left: 270px;
  margin-right: 35px;
  width: 100%;
  height: 100%;
  padding-bottom: 55px;
`;

// /* --------------------  CONTENT =============== */

export const AdPageContent = styled.div`
  width: 100%;
  background-color: #ffffff;
  border-radius: 16px;
  margin-top: 28px;
  padding: 25px 40px 30px 40px; /* Consistent padding */
`;

export const AdPageContent_Head = styled.div`
  margin-bottom: 30px;
  display: flex;
  justify-content: space-between;
  align-items: center;
`;

export const SearchArea = styled.div`
  width: 30%;

  display: flex;
  align-items: center;

  .searchInputContainer {
    display: flex;
    align-items: center;
    border-radius: 4px;
    padding: 4px 8px;
    width: 100%; // Full width within SearchArea
  }
  .searchIcon {
    margin: 0px 10px 0px 10px;
    color: #151542;
  }
  .searchInput {
    border: none;
    outline: none;
    flex-grow: 1;
    background-color: #f8f9fb;
    padding: 4px 8px;
    border: 1px solid rgba(203, 210, 220, 0.5);
    color: #151542;
    background-color: #f8f9fb;
    height: 45px;
    width: 100%; // Ensure input takes full space
  }
`;

export const AddButton = styled.div`
  button {
    background-color: ${theme.color.primary};
    color: #ffffff;
    width: 100%;
    height: 40px;
    border-radius: 10px;
    font-size: 12px;
    border: 1px solid ${theme.color.primary};
    padding: 10px 15px 10px 15px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    cursor: pointer;
    display: flex;
    align-items: center;
    justify-content: center;
  }

  button:hover {
    transition: all 0.4s;
    background-color: #ffffff;
    color: ${theme.color.primary};
    border: 1px solid ${theme.color.primary};
  }

  button .anticon {
    margin-right: 7px;
  }
`;

export const AdminTable = styled.div`
  padding: 0px 50px 0px 50px;

  table {
    border-collapse: collapse;
    width: 100%;
  }
  th,
  td {
    padding: 15px 0px 10px 0px;
    text-align: left;
    font-size: 16px;
    color: ${theme.color.primary};
  }
  th {
    color: #783232;
    font-size: 20px;
  }
  tr th {
    font-size: 13px;
    color: #92929d !important;
  }
  tr .TextAlign {
    text-align: center;
  }
  td .anticon {
    cursor: pointer;
  }
  .pendStatus {
    background-color: #f8e7ee;
    border-radius: 100px;
    padding: 5px 10px 5px 10px;
    font-size: 12px;
    color: #cd486b;
    border: none;
  }
  .confirmBtn {
    background-color: ${theme.color.primary};
    border-radius: 100px;
    padding: 7px 17px 7px 17px;
    box-shadow: 0px 4px 4px 0px rgba(0, 0, 0, 0.08);
    color: ${theme.color.secondary};
    border: none;
  }
  .confirmBtn:hover {
    cursor: pointer;
  }

  .AdPageContent_Content {
    display: flex;
    flex-direction: column; /* Stack vertically */
    gap: 24px; /* Consistent spacing between items */
    width: 100%; /* Ensure full width */
    padding: 0 0 20px 0; /* Add bottom padding for spacing */
  }
`;

export const AddContent_Title = styled.div`
  width: 100%;
  background-color: #ffffff;
  color: ${theme.color.primary};
  padding: 0px 0px 15px 0px; /* Add bottom padding for spacing */
  font-weight: 600;
  font-size: 18px;
`;

export const FormItem = styled.div`
  width: 100%;
  margin-bottom: auto;

  .ant-form-item {
    margin-bottom: 0 !important; /* Let FormItem handle spacing */
  }
`;

export const FormDescript = styled.div`
  width: 100%;
  textarea {
    height: 149px;
  }
`;

export const UploadFile = styled.div`
  width: 48%;
`;

export const ActionBtn = styled.div`
  margin-top: 25px;

  button {
    border: 1px solid ${theme.color.primary};
  }

  .ant-btn-primary {
    background-color: ${theme.color.primary};
    color: #92929d;
    border: 0px;
  }
`;