import { useEffect, useState } from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Login from './components/Login/Login';
import Register from './components/Register/Register';
import Navbar from './components/Navbar/Navbar';
import Urls from './components/Urls/Urls'
import Details from './components/UrlDetails/Details'

function App() {
    const [urls, setUrls] = useState([]);   

   return(
    <Router>
      <>
        <header>
          <Navbar />
        </header>

        <Routes>
          {/* <Route path="/" element={<Home />} /> */}
          <Route path="/urls/details/:id" element={<Details />}/>
          <Route path="/urls" element={<Urls />}/>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
        </Routes>        
      </>
    </Router>
   );
}

export default App;