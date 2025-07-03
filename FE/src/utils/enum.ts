export enum PageEnum {
    LOGIN = 'Login',
    REGISTER = 'Register',
    FORGOT_PASSWORD = 'ForgotPassword',
    SET_PASSWORD = 'SetPassword'
}

export const Role: {[key: string]: string} = {
    Customer: 'Customer',
    DeliveryStaff: 'DeliveryStaff',
    SalesStaff: 'SalesStaff',
    StoreManager: 'StoreManager',
    HeadOfficeAdmin: 'HeadOfficeAdmin'
}

export enum LinkEnum {
    LINK = 'Link',
    NAV_LINK = 'NavLink'
}

export enum CustomAction {
    SET_SELECTED_DIAMOND = 'SetSelectedDimond',
    SET_SELECTED_SETTING = 'SetSelectedSetting',
    SET_COMPLETED_RING = 'SetCompletedRing'
}

export enum PaymentMethodEnum {
    VNPAY = 'VNPay',
    MOMO = 'Momo',
    PAYPAL = 'Paypal',
    COD = 'COD'
}

export enum OrderStatus {
    PENDING = 'Pending',
    ACCEPTED = 'Accepted',
    ASSIGNED = 'Assigned',
    DELIVERING = 'Delivering',
    DELIVERED = 'Delivered',
    COMPLETED = 'Completed',
    CANCELLED = 'Cancelled'
}

