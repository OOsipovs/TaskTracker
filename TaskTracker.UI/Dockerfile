FROM node:20-alpine

WORKDIR /app

# Copy package files
COPY TaskTracker.UI/package*.json ./

# Install dependencies
RUN npm install

# Copy source code
COPY TaskTracker.UI/ ./

# Expose port
EXPOSE 4200

# Start Angular development server
CMD ["npm", "start", "--", "--host", "0.0.0.0", "--poll", "2000"]