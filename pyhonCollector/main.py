import uvicorn
import psutil
import os
from fastapi import FastAPI
from fastapi.responses import PlainTextResponse

app = FastAPI()


def get_disk_memory() -> (float, float):
    result = os.statvfs('/')
    block_size = result.f_frsize
    total_blocks = result.f_blocks
    free_blocks = result.f_bfree
    giga = 1024 * 1024
    # giga = 1000 * 1000 * 1000
    total_size = total_blocks * block_size / giga
    free_size = free_blocks * block_size / giga
    return total_size, free_size


def get_ram_memory() -> (float, float):
    mem = psutil.virtual_memory()
    total = mem.total / 1024 / 1024
    free = (mem.available - mem.inactive) / 1024 / 1024
    return total, free


def get_metrics():
    cpu_temp = 1
    gpu_freq = 3
    gpu_temp = 2

    memory_total_ram, memory_available_ram = get_ram_memory()
    memory_total_disk, memory_available_disk = get_disk_memory()

    text = '# HELP temperature Current element temperature.\n'
    text += '# TYPE temperature gauge\n'
    text += 'temperature{target=\"CPU\"} ' + f'{cpu_temp}\n'
    text += 'temperature{target=\"GPU\"} ' + f'{gpu_temp}\n'

    text += '# HELP percent_usage Represents current elements usage in %.\n'
    text += '# TYPE percent_usage gauge\n'
    cores = psutil.cpu_percent(percpu=True)
    i = 0
    for core in cores:
        text += 'percent_usage{target=\"CPU\",' + f'core=\"{i}\"' + '} ' + f'{core}\n'
        i += 1
    text += 'percent_usage{target=\"GPU\"} ' + f'{gpu_freq}\n'

    text += '# HELP memory_total Represents total memory.\n'
    text += '# TYPE memory_total gauge\n'
    text += 'memory_total{target=\"RAM\"} ' + f'{memory_total_ram}\n'
    text += 'memory_total{target=\"DISK\"} ' + f'{memory_total_disk}\n'

    text += '# HELP memory_available Represents available memory.\n'
    text += '# TYPE memory_available gauge\n'
    text += 'memory_available{target=\"RAM\"} ' + f'{memory_available_ram}\n'
    text += 'memory_available{target=\"DISK\"} ' + f'{memory_available_disk}'
    return text


@app.get("/metrics", response_class=PlainTextResponse)
def read_root():
    r = psutil.virtual_memory()
    print(f'total: {r.total / 1024 / 1024}')
    print(f'used: {r.used / 1024 / 1024}')
    print(f'available: {r.available / 1024 / 1024}')
    print(f'inactive: {r.inactive / 1024 / 1024}')
    print(f'percent: {r.percent}')
    print(f'free: {r.free / 1024 / 1024}')
    print(f'active: {r.active / 1024 / 1024}')
    return get_metrics()


if __name__ == "__main__":
    uvicorn.run(app, host="127.0.0.1", port=5213)
