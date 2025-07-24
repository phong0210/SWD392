import { Input } from "antd";
import * as Styled from "./FooterTop.styled";
import {
  FacebookFilled,
  InstagramFilled,
  MailFilled,
  PhoneFilled,
  TwitterCircleFilled,
} from "@ant-design/icons";

const NavigationContent = [
  {
    id: 1,
    title: "Navigation",
    subCategory: [
      {
        id: 1,
        subTitle: "Diamond",
      },
      {
        id: 3,
        subTitle: "Learn About",
      },
      {
        id: 4,
        subTitle: "About Us",
      },
    ],
  },
  {
    id: 2,
    title: "Customer Care",
    subCategory: [
      {
        id: 1,
        subTitle: "(000) 000 000",
        icon: <PhoneFilled />,
      },
      {
        id: 2,
        subTitle: "Our Service",
        icon: <MailFilled />,
      },
      {
        id: 3,
        subTitle: "FAQs",
      },
      {
        id: 4,
        subTitle: "Our Service",
      },
    ],
  },
  {
    id: 3,
    title: "Diamond Shape",
    subCategory: [
      {
        id: 1,
        subTitle: "Round",
      },
      {
        id: 2,
        subTitle: "Princess",
      },
      {
        id: 3,
        subTitle: "Cushion",
      },
      {
        id: 4,
        subTitle: "Oval",
      },
      {
        id: 5,
        subTitle: "Emerald",
      },
      {
        id: 6,
        subTitle: "Pear",
      },
      {
        id: 7,
        subTitle: "Asscher",
      },
      {
        id: 8,
        subTitle: "Heart",
      },
      {
        id: 9,
        subTitle: "Radiant",
      },
      {
        id: 10,
        subTitle: "Marquise",
      },
    ],
  },
];

const FooterTop = () => {
  return (
    <>
      <Styled.TopContainer>
        <Styled.TopFlexbox>
          <Styled.ContactUs>
            <Styled.TitleCategory>Information</Styled.TitleCategory>
            <Styled.DescriptionContact>
              Welcome to APHROMAS, where elegance meets authenticity.
              We offer certified natural diamonds with premium quality and timeless beauty.
              Explore our exclusive collection and let each diamond tell your story.
            </Styled.DescriptionContact>
            <Input
              size="large"
              placeholder="large size"
              prefix={<MailFilled />}
            ></Input>
            <Styled.SNSContainer>
              <FacebookFilled />
              <InstagramFilled />
              <TwitterCircleFilled />
            </Styled.SNSContainer>
          </Styled.ContactUs>
          <Styled.CategoryContainer>
            {NavigationContent.map((items) => (
              <Styled.NavigationTitle key={items.id}>
                <Styled.TitleCategory>{items.title}</Styled.TitleCategory>
                <Styled.FooterElement>
                  {items.subCategory.map((subItems) => (
                    <Styled.NavElement key={subItems.id}>
                      {subItems.icon} {subItems.subTitle}
                    </Styled.NavElement>
                  ))}
                </Styled.FooterElement>
              </Styled.NavigationTitle>
            ))}
          </Styled.CategoryContainer>
        </Styled.TopFlexbox>
      </Styled.TopContainer>
    </>
  );
};

export default FooterTop;
