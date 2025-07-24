import { FieldType } from "@/components/AuthForm/AuthForm.fields";
import { Input, InputNumber, Select, Switch } from "antd";
import {
  Category_Option,
  ClarityType_Option,
  ColorType,
  FluorescenceType_Option,
  RateType_Option,
  ShapeType,
} from "../../Diamond.type";
import TextArea from "antd/es/input/TextArea";

export const DiamondField: FieldType[] = [
  {
    key: 1,
    label: "Diamond Name",
    name: "name",
    rules: [
      {
        required: true,
        message: "Diamond Name is required.",
      },
      {
        pattern: /^[a-zA-Z0-9\s()-.]*$/,
        message: "Only alphabet, numbers, (), - and . are allowed.",
      },
      {
        max: 300,
        message: "Diamond Name must be at most 300 characters long.",
      },
      {
        min: 2,
        message: "Diamond Name must be at least 2 characters",
      },
      {
        whitespace: true,
        message: "Not only has whitespace.",
      },
    ],
    children: <Input className="formItem" placeholder="Fill Name" />,
  },
  {
    key: 2,
    label: "Sku",
    name: "sku",
    rules: [
      {
        required: true,
        message: "Sku is required.",
      },
      {
        pattern: /^[a-zA-Z0-9\s()-.]*$/,
        message: "Only alphabet, numbers, (), - and . are allowed.",
      },
      {
        max: 300,
        message: "Sku must be at most 300 characters long.",
      },
      {
        min: 2,
        message: "Sku must be at least 2 characters",
      },
      {
        whitespace: true,
        message: "Not only has whitespace.",
      },
    ],
    children: <Input className="formItem" placeholder="Sku" />,
  },
  {
    key: 3,
    label: "Price",
    name: "price",
    rules: [
      {
        required: true,
        message: "Price is required.",
      },
      {
        type: "number",
        min: 0,
        max: 1000000,
        message:
          "Must be a positive number and less than or equal to $1,000,000 USD.",
      },
    ],
    children: <InputNumber className="formItem" placeholder="4,080" />,
  },
  {
    key: 4,
    label: "Color",
    name: "color",
    rules: [
      {
        required: true,
      },
    ],
    children: (
      <Select
        className="formItem"
        placeholder="Select Color"
        options={ColorType}
      />
    ),
  },
  {
    key: 5,
    label: "Cut",
    name: "cut",
    rules: [
      {
        required: true,
      },
    ],
    children: (
      <Select
        className="formItem"
        placeholder="Select Cut"
        options={RateType_Option}
      />
    ),
  },
  {
    key: 6,
    label: "Clarity",
    name: "clarity",
    rules: [
      {
        required: true,
      },
    ],
    children: (
      <Select
        className="formItem"
        placeholder="Select Clarity"
        options={ClarityType_Option}
      />
    ),
  },

  {
    key: 7,
    label: "Carat",
    name: "carat",
    rules: [
      {
        required: true,
      },
      {
        min: 0.5,
        max: 10,
        type: "number",
      },
    ],
    children: <InputNumber className="formItem" placeholder="1,01" />,
  },
  {
    key: 8,
    label: "Quantity",
    name: "stockQuantity",
    rules: [
      {
        required: true,
      },
      {
        min: 0.5,
        max: 10,
        type: "number",
      },
    ],
    children: <InputNumber className="formItem" placeholder="1,01" />,
  },
  {
    key: 9,
    label: "Category",
    name: "categoryId",
    rules: [
      {
        required: true,
      },
    ],
    children: (
      <Select
        className="formItem"
        placeholder="Select Category"
        options={Category_Option}
      />
    ),
  },
  {
    key: 10,
    label: "Description",
    name: "description",
    rules: [
      {
        required: true,
        message: "Description is required.",
      },
      {
        pattern: /^[a-zA-Z0-9\s()-.]*$/,
        message: "Only alphabet, numbers, (), - and . are allowed.",
      },
      {
        whitespace: true,
        message: "Not only has whitespace.",
      },
    ],
    children: <TextArea placeholder="Description" allowClear />,
  },
  {
    key: 11,
    label: "GIACertNumber",
    name: "giaCertNumber",
    rules: [
      {
        required: true,
        message: "GIA is required.",
      },
      {
        pattern: /^[a-zA-Z0-9\s()-.]*$/,
        message: "Only alphabet, numbers, (), - and . are allowed.",
      },
      {
        whitespace: true,
        message: "Not only has whitespace.",
      },
    ],
    children: <TextArea placeholder="GIACertNumber" allowClear />,
  },
  {
    key: 12,
    label: "Active",
    name: "isHidden",
    rules: [],
    children: <Switch defaultChecked defaultValue={true} />,
  },
];
