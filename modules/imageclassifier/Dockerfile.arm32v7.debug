FROM arm32v7/python:3.9-slim-bookworm

RUN pip install --no-cache-dir "flask<3" "pillow<11" "numpy<2" tflite-runtime~=2.13.0 --extra-index-url=https://www.piwheels.org/simple

COPY app /app
EXPOSE 80
WORKDIR /app

CMD python -u app.py
