import uvicorn
import psutil
from fastapi import FastAPI
from fastapi.responses import PlainTextResponse


app = FastAPI()


def get_metrics():
    cpu_freq = 4
    gpu_freq = 3
    cpu_temp = 1
    gpu_temp = 2
    text = '# HELP temperature Current element temperature.\n'
    text += '# TYPE temperature gauge\n'
    text += 'temperature{target=\"CPU\"} ' + f'{cpu_temp}\n'
    text += 'temperature{target=\"GPU\"} ' + f'{gpu_temp}\n'
    text += '# HELP speed Represents current elements speed.\n'
    text += '# TYPE speed gauge\n'
    text += 'speed{target=\"CPU\"} ' + f'{cpu_freq}\n'
    text += 'speed{target=\"GPU\"} ' + f'{gpu_freq}\n'
    text += '# HELP memory Represents available memory.\n'
    text += '# TYPE memory gauge\n'
    text += 'memory{target=\"RAM\"} ' + f'{0}\n'
    text += 'memory{target=\"DISK\"} ' + f'{0}'
    return text


@app.get("/metrics", response_class=PlainTextResponse)
def read_root():
    return get_metrics()


if __name__ == "__main__":
    uvicorn.run(app, host="127.0.0.1", port=5213)

