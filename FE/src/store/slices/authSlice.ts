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
  loading: true, // Start with loading true
  error: null,
};

export const login = createAsyncThunk(
  "auth/login",
  async (credentials: any, { rejectWithValue }) => {
    try {
      const { data } = await loginAPI(credentials);
      if (!data || !data.token) throw new Error("Invalid response");

      cookieUtils.setToken(data.token);
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

export const initializeAuth = createAsyncThunk(
  "auth/initialize",
  async (_, { rejectWithValue }) => {
    try {
      const token = cookieUtils.getToken();
      if (token) {
        const decoded = cookieUtils.decodeJwt() as any;
        if (decoded && decoded.exp > Date.now() / 1000) {
          return {
            token,
            user: {
              email: decoded.sub,
              fullName: decoded.sub,
              role: decoded["HeadOfficeAdmin"] || "HeadOfficeAdmin",
              userId: decoded.AccountID,
            },
          };
        }
      }
      return { token: null, user: null };
    } catch (error) {
      cookieUtils.clear();
      return rejectWithValue("Invalid token");
    }
  }
);

const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    logout(state) {
      cookieUtils.clear();
      state.user = null;
      state.token = null;
      state.error = null;
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
      })
      .addCase(initializeAuth.pending, (state) => {
        state.loading = true;
      })
      .addCase(initializeAuth.fulfilled, (state, action) => {
        state.user = action.payload.user;
        state.token = action.payload.token;
        state.loading = false;
      })
      .addCase(initializeAuth.rejected, (state) => {
        state.user = null;
        state.token = null;
        state.loading = false;
      });
  },
});

export const { logout } = authSlice.actions;
export { authSlice };
