import { theme } from "@/themes";
import styled from "styled-components";
import { createGlobalStyle } from "styled-components";

export const GlobalStyle = createGlobalStyle`
  html, body {
    height: 100%;
    margin: 0;
    padding: 0;
    background-color: #f1f1f1;
    font-family: 'Poppins', sans-serif;
  }
`;

export const PageAdminArea = styled.section`
  display: inline-flex;
  background-color: #f1f1f1;
  font-family: "Poppins", sans-serif;
  /* height: 100%; */
  width: 100%;
`;

export const AdminPage = styled.div`
  margin-left: 270px;
  margin-right: 35px;
  width: 100%;
  height: 100%;
  padding-bottom: 55px;
`;

export const TitlePage = styled.div`
  h1 {
    font-size: 22px;
    font-weight: 36px;
    color: #151542;
    margin: 30px 0px 0px 0px;
    padding-bottom: 7px;
  }

  p {
    color: #92929d;
    font-size: 13px;
    margin: 0px 0px 0px 0px;
  }
`;

export const PageContent = styled.div`
  width: 100%;
  border-radius: 16px;
  padding-bottom: 30px;
`;

// export const PageDetail_Title = styled.div`
//   width: 100%;
//   font-size: 18px;
//   font-weight: 600;
//   margin-bottom: 25px;
//   color: ${theme.color.primary};
// `;

export const PageDetail_Infor = styled.div`
  width: 100%;
  height: 80%;
  display: flex;
  flex-direction: column;
  justify-content: space-between;

  .InforLine_Title-Edit {
  .ant-col {
  width: 100%;
  text-align: left;
  }
              

  }
`;

export const PageDetail_Title = styled.div`
  width: 100%;
  display: flex;
  align-items: flex-end; // thụt xuống dưới một chút
  gap: 12px;
  margin-bottom: 25px;
  margin-top: 12px; // thêm khoảng cách với phía trên nếu cần

  .icon {
    font-size: 22px;
    margin-bottom: 2px; // để icon cũng trùng baseline
  }

  .text {
    font-size: 22px;
    font-weight: 700;
    color: ${theme.color.primary};
    position: relative;
    padding-bottom: 6px; // đẩy gạch dưới cách text
  }

  .text::after {
    content: "";
    display: block;
    width: 60%; // 👉 tăng chiều dài underline ở đây
    height: 3px;
    background-color: ${theme.color.primary};
    position: absolute;
    bottom: 0;
    left: 0;
    border-radius: 2px;
  }
`;


export const MaterialTable = styled.div`

margin: 20px 0px;
`;

export const InforLine = styled.div`
  display: flex;
  justify-content: space-between;
  margin-bottom: 20px;
  font-size: 15px;

  .InforLine_Title {
    font-weight: 600;
  }
    label {

  }
`;

export const InforLine_Descrip = styled.div`
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  margin-bottom: 20px;
  font-size: 15px;

  .InforLine_Title {
    font-weight: 600;

  }
`;

export const PageContent_Bot = styled.div`
  width: 100%;
  background-color: #ffffff;
  border-radius: 16px;
  margin-top: 28px;
  padding: 25px 40px 20px 40px;
`;

export const ActionBtn = styled.div`
  margin-top: 25px;
  display: flex;
  justify-content: space-between;
`;

export const ActionBtn_Left = styled.div`

  .MainBtn {
    background-color: ${theme.color.primary};
    color: #ffffff;
  }
`;

export const ActionBtn_Right = styled.div`
  .DeleteBtn {
    background-color: #de5353;
    color: #ffffff;
  }
`;
