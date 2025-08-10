import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Navigation from './components/Navigation/Navigation';
import AsyncDemo from './components/AsyncDemo/AsyncDemo'; // NOWY
import Home from './components/Home/Home';
import './App.css';

const App: React.FC = () => {
  return (
    <Router>
      <div className="App">
        <Navigation />
        <main className="main-content">
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/async-demo" element={<AsyncDemo />} /> {/* NOWY */}
          </Routes>
        </main>
      </div>
    </Router>
  );
};

export default App;
