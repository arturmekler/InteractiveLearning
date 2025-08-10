import React, { useState } from 'react';
import { asyncDemoService } from '../../services/apiService';
import type { 
  ThreadInfo, 
  ComparisonResult, 
  ParallelTasksResult, 
  TaskLifecycleResult,
  ThreadPoolResult,
  ConfigureAwaitResult,
  CancellationResult 
} from '../../services/apiService';
import LoadingSpinner from '../Common/LoadingSpinner';
// import './AsyncDemo.css';

interface DemoState {
  threadInfo: ThreadInfo | null;
  comparison: ComparisonResult | null;
  parallelTasks: ParallelTasksResult | null;
  taskLifecycle: TaskLifecycleResult | null;
  threadPool: ThreadPoolResult | null;
  configureAwait: ConfigureAwaitResult | null;
  cancellation: CancellationResult | null;
  loading: Record<string, boolean>;
}

const AsyncDemo: React.FC = () => {
  const [state, setState] = useState<DemoState>({
    threadInfo: null,
    comparison: null,
    parallelTasks: null,
    taskLifecycle: null,
    threadPool: null,
    configureAwait: null,
    cancellation: null,
    loading: {}
  });

  const [abortController, setAbortController] = useState<AbortController | null>(null);

  const setLoading = (key: string, value: boolean) => {
    setState(prev => ({
      ...prev,
      loading: { ...prev.loading, [key]: value }
    }));
  };

  const handleThreadInfo = async () => {
    setLoading('threadInfo', true);
    try {
      const response = await asyncDemoService.getThreadInfo();
      setState(prev => ({ ...prev, threadInfo: response.data }));
    } catch (error) {
      console.error('Thread info error:', error);
    } finally {
      setLoading('threadInfo', false);
    }
  };

  const handleSyncVsAsync = async () => {
    setLoading('comparison', true);
    try {
      const response = await asyncDemoService.compareSyncVsAsync();
      setState(prev => ({ ...prev, comparison: response.data }));
    } catch (error) {
      console.error('Sync vs Async error:', error);
    } finally {
      setLoading('comparison', false);
    }
  };

  const handleComprehensive = async () => {
    setLoading('comprehensive', true);
    try {
      await Promise.all([
        handleThreadInfo(),
        handleSyncVsAsync(),
      ]);
    } finally {
      setLoading('comprehensive', false);
    }
  };

  return (
    <div className="async-demo">
      <header className="page-header">
        <h1>🔧 Zaawansowana Demonstracja Asynchroniczności</h1>
        <p>Kompleksowe przykłady threads, tasks i async/await w .NET</p>
      </header>

      <div className="demo-section">
        <div className="demo-card full-width comprehensive-card">
          <h3>🎯 Kompleksowa Demonstracja</h3>
          <p>Uruchom demonstracje asynchroniczności</p>
          <button 
            onClick={handleComprehensive}
            disabled={state.loading.comprehensive}
            className="demo-button comprehensive-button"
          >
            {state.loading.comprehensive ? <LoadingSpinner /> : 'Uruchom Demo'}
          </button>
        </div>
      </div>

      <div className="demo-section">
        <div className="demo-card">
          <h3>🧵 Informacje o Wątkach</h3>
          <p>Sprawdź jak await wpływa na wątki</p>
          <button 
            onClick={handleThreadInfo}
            disabled={state.loading.threadInfo}
            className="demo-button thread-button"
          >
            {state.loading.threadInfo ? <LoadingSpinner /> : 'Sprawdź Wątki'}
          </button>
          {state.threadInfo && (
            <div className="result-display">
              <div className="thread-info">
                <div className="thread-item">
                  <span className="label">Przed await:</span>
                  <span className="value">Thread {state.threadInfo.threadBeforeAwait}</span>
                </div>
                <div className="thread-item">
                  <span className="label">Po await:</span>
                  <span className="value">Thread {state.threadInfo.threadAfterAwait}</span>
                </div>
                <div className="thread-item">
                  <span className="label">ThreadPool:</span>
                  <span className={`value ${state.threadInfo.isThreadPoolThread ? 'yes' : 'no'}`}>
                    {state.threadInfo.isThreadPoolThread ? 'Tak' : 'Nie'}
                  </span>
                </div>
                <div className="explanation">
                  💡 {state.threadInfo.explanation}
                </div>
              </div>
            </div>
          )}
        </div>

        <div className="demo-card">
          <h3>⚡ Sync vs Async</h3>
          <p>Porównanie wydajności</p>
          <button 
            onClick={handleSyncVsAsync}
            disabled={state.loading.comparison}
            className="demo-button comparison-button"
          >
            {state.loading.comparison ? <LoadingSpinner /> : 'Porównaj'}
          </button>
          {state.comparison && (
            <div className="result-display">
              <div className="performance-bars">
                <div className="performance-item">
                  <span>Synchroniczne:</span>
                  <div className="bar sync-bar">
                    <span>{state.comparison.synchronousTime}ms</span>
                  </div>
                </div>
                <div className="performance-item">
                  <span>Asynchroniczne:</span>
                  <div className="bar async-bar">
                    <span>{state.comparison.asynchronousTime}ms</span>
                  </div>
                </div>
              </div>
              <div className="explanation">
                💡 {state.comparison.explanation}
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default AsyncDemo;
