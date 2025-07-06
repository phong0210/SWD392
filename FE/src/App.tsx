import { BrowserRouter as Router } from 'react-router-dom'
import { useEffect } from 'react'
import { useDispatch } from 'react-redux'
import { initializeAuth } from './store/slices/authSlice'
import RoutesComponent from './routes'

const App = () => {
  const dispatch = useDispatch();

  useEffect(() => {
    dispatch(initializeAuth());
  }, [dispatch]);

  return (
    <>
      <Router>
        <RoutesComponent />
      </Router>

    </>
  )
}

export default App
