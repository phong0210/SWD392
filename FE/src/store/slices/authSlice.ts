import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import { login as loginAPI } from "@/services/authAPI";
import cookieUtils from "@/services/cookieUtils";

interface AuthState {
  user: any;
  token: string | null;
  loading: boolean;
  error: string | null;
}

const initialState: AuthState = {
  user: null,
  token: null,
  loading: false,
  error: null,
};

export const login = createAsyncThunk(
  "auth/login",
  async (credentials: any, { rejectWithValue }) => {
    try {
      const { data } = await loginAPI(credentials);
      if (!data || !data.token) throw new Error("Invalid response");

      // Store token in cookies
      cookieUtils.setToken(data.token);

      // Decode token to get user info
      const decoded = cookieUtils.decodeJwt() as any;

      const role = decoded["Role"] || "Customer";

      return {
        token: data.token,
        user: {
          email: decoded.sub,
          fullName: data.user?.name || decoded.sub,
          role: role,
          userId: decoded.AccountID,
        },
      };
    } catch (err: any) {
      return rejectWithValue(err.response?.data || err.message);
    }
  }
);

const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    logout(state) {
      state.user = null;
      state.token = null;
      state.error = null;
    },
    initializeAuth(state) {
      const token = cookieUtils.getToken();
      if (token) {
        try {
          const decoded = cookieUtils.decodeJwt() as any;
          if (decoded && decoded.exp > Date.now() / 1000) {
            state.token = token;
            state.user = {
              email: decoded.sub,
              fullName: decoded.sub, // You might want to get this from API
              role: decoded["Role"] || "Customer",
              userId: decoded.AccountID,
            };
          } else {
            cookieUtils.clear();
          }
        } catch (error) {
          // Token is invalid, clear it
          cookieUtils.clear();
        }
      }
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(login.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(login.fulfilled, (state, action) => {
        state.loading = false;
        state.user = action.payload.user || null;
        state.token = action.payload.token;
        state.error = null;
      })
      .addCase(login.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });
  },
});

export const { logout, initializeAuth } = authSlice.actions;
export { authSlice };
