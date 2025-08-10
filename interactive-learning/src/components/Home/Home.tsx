import React from 'react';
import './Home.css';

const Home: React.FC = () => {
  return (
    <div className="home">
      <header className="page-header">
        <h1>🔄 Async Programming Demo</h1>
        <p>Interaktywna platforma do nauki programowania asynchronicznego w .NET</p>
      </header>
      
      <div className="features-grid">
        <div className="feature-card highlight">
          <span className="feature-icon">🎯</span>
          <h3>Advanced Demo</h3>
          <p>Kompleksowe przykłady threads, tasks i ThreadPool w .NET</p>
          <ul className="feature-list">
            <li>🧵 Thread Management</li>
            <li>⚡ Sync vs Async Performance</li>
            <li>🚀 Parallel Tasks</li>
            <li>📋 Task Lifecycle</li>
            <li>🏊 ThreadPool Demo</li>
            <li>⚙️ ConfigureAwait</li>
            <li>❌ Cancellation Tokens</li>
          </ul>
        </div>
        
        <div className="info-card">
          <h3>🔧 Co zobaczysz?</h3>
          <ul>
            <li><strong>Real-time metryki</strong> wydajności</li>
            <li><strong>Wizualizację</strong> przełączania wątków</li>
            <li><strong>Porównania</strong> sync vs async</li>
            <li><strong>Demonstrację</strong> ThreadPool</li>
            <li><strong>Anulowanie</strong> długich operacji</li>
            <li><strong>Best practices</strong> async/await</li>
          </ul>
        </div>
      </div>
    </div>
  );
};

export default Home;
